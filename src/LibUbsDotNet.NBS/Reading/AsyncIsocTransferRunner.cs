using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using LibUsbDotNet.Main;

namespace LibUsbDotNet.LibUsb
{
	public class AsyncIsocTransferRunner<TOutput> : IDisposable
	{
		public delegate ValueTask ReceptionHandler(in TOutput data);
		public delegate void ErrorHandler(Error error);

        private int m_disposed;
        private int m_started;

        private readonly ReceptionHandler m_receptionHandler;
        private readonly ErrorHandler m_errorHandler;

        private readonly IntPtr[] m_transfers;
        private readonly TransferManagement<TOutput>[] m_managements;
		private readonly ValueTask[] m_receptionTasks;
		private readonly int m_numIsoPacketPerTransfer;

		private readonly UsbContext m_context;
        
        public unsafe AsyncIsocTransferRunner(UsbDevice usbDevice, byte endPoint, int isoPacketSize, int numIsoPacketPerTransfer, TransferManagement<TOutput>[] managements, ReceptionHandler receptionHandler, ErrorHandler errorHandler)
        {
	        m_context = usbDevice.Context;
	        m_managements = managements;
			m_receptionHandler = receptionHandler;
			m_errorHandler = errorHandler;
	        
			var parallelTransferCount = managements.Length;
	        m_transfers = new IntPtr[parallelTransferCount];

	        var deviceHandle = usbDevice.DeviceHandle.DangerousGetHandle();

	        m_numIsoPacketPerTransfer = numIsoPacketPerTransfer;
	        m_receptionTasks = new ValueTask[numIsoPacketPerTransfer];
	        
            for (var i = 0; i < m_transfers.Length; ++i)
            {
	            var transfer = NativeMethods.AllocTransfer(numIsoPacketPerTransfer);
	            transfer->DevHandle = deviceHandle;
	            transfer->Endpoint = endPoint;
	            transfer->Timeout = 0;
	            transfer->Type = (byte)EndpointType.Isochronous;
	            transfer->NumIsoPackets = numIsoPacketPerTransfer;
	            
	            transfer->Flags = (byte)TransferFlags.None;
	            
	            transfer->Callback = Marshal.GetFunctionPointerForDelegate(NativeMethods.TransferDelegate(Callback));
	            
	            transfer->UserData = new IntPtr(i);

	           var descriptors = (IsoPacketDescriptor*)(&transfer->NumIsoPackets + 1);
		            
	            for (var p = 0; p < numIsoPacketPerTransfer; ++p)
	            {
		            descriptors[p].Length = (uint)isoPacketSize;
		            
	            }
				
                m_transfers[i] = new IntPtr(transfer);
            }
        }
        
        public void StartReceive()
        {
	        if (Interlocked.Exchange(ref m_started, 1) > 0)
	        {
                return;
	        }

	        m_context.NeedsEventHandling();

	        unsafe
	        {
		        for (var i = 0; i < m_transfers.Length; ++i)
		        {
			        Submit((Transfer*) m_transfers[i].ToPointer());
		        }
	        }
        }

        private unsafe void Submit(Transfer* transfer)
        {
	        var transferId = transfer->UserData.ToInt32();
	        
            var (bufferPtr, length) = m_managements[transferId].PrepareTransfer();

            transfer->Buffer = (byte*)bufferPtr.ToPointer();
            transfer->Length = length;

            var status = NativeMethods.SubmitTransfer(transfer);
            if (status == Error.Success)
            {
	           return;
            }

            m_transfers[transfer->UserData.ToInt32()] = IntPtr.Zero;
            NativeMethods.FreeTransfer(transfer);

            m_errorHandler(status);
        }

        private unsafe void Callback(Transfer* transfer)
        {
	        var status = transfer->Status;

	        if (status == TransferStatus.Cancelled)
	        {
				Console.WriteLine($"Free! {transfer->UserData.ToInt32()}");
				m_transfers[transfer->UserData.ToInt32()] = IntPtr.Zero;
		        NativeMethods.FreeTransfer(transfer);
		        return;
	        }

            if (status != TransferStatus.Completed)
            {
	            m_errorHandler(Error.Other); //TODO BETTER
                return;
            }
			
            var transferId = transfer->UserData.ToInt32();

            var descriptors = (IsoPacketDescriptor*) (&transfer->NumIsoPackets + 1);
			var offset = 0;
			for (var p = 0; p < m_numIsoPacketPerTransfer; ++p)
			{
				var descriptor = descriptors[p];
				if (descriptor.Status == TransferStatus.Completed)
				{
					m_receptionTasks[p] = m_receptionHandler(m_managements[transferId].HandleTransferCompleted(offset, (int) descriptor.ActualLength));
				}
				else
				{
					m_receptionTasks[p] = default;
				}

				offset += (int) descriptor.Length;
			}

			if(m_receptionTasks.All(t => t.IsCompleted))
			{
                Submit(transfer);
                return;
            }

			Task.WhenAll(m_receptionTasks.Select(t => t.AsTask())).ContinueWith(_ =>
            {
	            Submit(transfer);
            });
        }

		public unsafe void Dispose()
        {
	        if (Interlocked.Exchange(ref m_disposed, 1) > 0)
	        {
                return;
	        }

			int stillPendingTransfer;

			do
			{
				stillPendingTransfer = 0;
				Console.WriteLine($"A");
				for (var i = 0; i < m_transfers.Length; ++i)
				{
					var transferPtr = m_transfers[i];
					if (transferPtr == IntPtr.Zero)
					{
						continue;
					}

					Console.WriteLine($"Cancel");
					var transfer = (Transfer*) transferPtr.ToPointer();
					NativeMethods.CancelTransfer(transfer);
					++stillPendingTransfer;
				}

				Console.WriteLine($"B {stillPendingTransfer}");
				Thread.Sleep(1);
			}
			while(stillPendingTransfer > 0);

	        m_context.DoesntNeedEventHandlingAnymore();
        }
	}
}
