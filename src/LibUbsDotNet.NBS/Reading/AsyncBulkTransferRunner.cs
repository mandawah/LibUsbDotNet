using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using LibUsbDotNet.Main;

namespace LibUsbDotNet.LibUsb
{
	public class AsyncBulkTransferRunner<TOutput> : IDisposable
	{
		public delegate ValueTask ReceptionHandler(in TOutput data);
		public delegate void ErrorHandler(Error error);

        private readonly int m_receptionBufferLength;

        private int m_disposed;
        private int m_started;

        private readonly ReceptionHandler m_receptionHandler;
        private readonly ErrorHandler m_errorHandler;

        private readonly IntPtr[] m_transfers;
        private readonly TransferManagement[] m_managements;

		private readonly UsbContext m_context;
        
        public unsafe AsyncBulkTransferRunner(UsbDevice usbDevice, byte endPoint, TransferManagement[] managements, ReceptionHandler receptionHandler, ErrorHandler errorHandler)
        {
	        m_context = usbDevice.Context;
	        m_managements = managements;
			m_receptionHandler = receptionHandler;
			m_errorHandler = errorHandler;
	        
			var parallelTransferCount = managements.Length;
	        m_transfers = new IntPtr[parallelTransferCount];

	        var deviceHandle = usbDevice.DeviceHandle.DangerousGetHandle();
	        
            for (var i = 0; i < m_transfers.Length; ++i)
            {
	            var transfer = NativeMethods.AllocTransfer(0);
	            transfer->DevHandle = deviceHandle;
	            transfer->Endpoint = endPoint;
	            transfer->Timeout = 0;
	            transfer->Type = (byte)EndpointType.Bulk;
	            transfer->NumIsoPackets = 0;
	            
	            transfer->Flags = (byte)TransferFlags.None;
	            
	            transfer->Callback = Marshal.GetFunctionPointerForDelegate(NativeMethods.TransferDelegate(Callback));
	            
	            transfer->UserData = new IntPtr(i);
                
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
	        
            var (bufferPtr, length) = m_managements[transferId].PrepareTransfer(m_receptionBufferLength);

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
	            m_errorHandler(Error.Other);
                return;
            }
			
            var transferId = transfer->UserData.ToInt32();

            var receptionTask = m_receptionHandler(m_managements[transferId].HandleTransferCompleted(transfer->ActualLength));

            if (receptionTask.IsCompleted)
            {
                Submit(transfer);
                return;
            }

            receptionTask.AsTask().ContinueWith(_ =>
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

		public readonly struct TransferManagement
		{
			public delegate (IntPtr bufferPtr, int bufferLength) PrepareTransferDelegate(int length);
			public delegate TOutput HandleTransferCompletedDelegate(int actualLength);
			
			public readonly PrepareTransferDelegate PrepareTransfer;
			public readonly HandleTransferCompletedDelegate HandleTransferCompleted;
			
			public TransferManagement(PrepareTransferDelegate prepareTransfer, HandleTransferCompletedDelegate handleTransferCompleted)
			{
				PrepareTransfer = prepareTransfer;
				HandleTransferCompleted = handleTransferCompleted;
			}
		}
	}
}
