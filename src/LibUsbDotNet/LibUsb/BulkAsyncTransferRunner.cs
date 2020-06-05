using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using LibUsbDotNet.Main;

namespace LibUsbDotNet.LibUsb
{
	class BulkAsyncTransferRunner<TOutput> : IDisposable where TOutput : struct
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
        
        public unsafe BulkAsyncTransferRunner(IntPtr deviceHandle, byte endPoint, TransferManagement[] managements, ReceptionHandler receptionHandler, ErrorHandler errorHandler)
        {
	        m_managements = managements;
			m_receptionHandler = receptionHandler;
			m_errorHandler = errorHandler;
	        
			var parallelTransferCount = managements.Length;
	        m_transfers = new IntPtr[parallelTransferCount];

            for (var i = 0; i < m_transfers.Length; ++i)
            {
	            var transfer = NativeMethods.AllocTransfer(0);
	            transfer->DevHandle = deviceHandle;
	            transfer->Endpoint = endPoint;
	            transfer->Timeout = 0;
	            transfer->Type = (byte)EndpointType.Bulk;
	            transfer->NumIsoPackets = 0;
	            
	            transfer->Flags = (byte)TransferFlags.None;
	            
	            transfer->Callback = Marshal.GetFunctionPointerForDelegate(new TransferDelegate(Callback));
	            
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

            m_errorHandler(status);
        }

        private unsafe void Callback(Transfer* transfer)
        {
	        var status = (Error)transfer->Status;

            if (status != Error.Success)
            {
	            m_errorHandler(status);
                return;
            }

            var transferId = transfer->UserData.ToInt32();

            var receptionTask = m_receptionHandler(
				m_managements[transferId].HandleTransferCompleted(
					new IntPtr(transfer->Buffer),
					transfer->ActualLength));

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

	        for (var i = 0; i < m_transfers.Length; ++i)
	        {
		        var transfer = (Transfer*) m_transfers[i].ToPointer();
		        NativeMethods.CancelTransfer(transfer);
		        NativeMethods.FreeTransfer(transfer);
	        }
        }

		public readonly struct TransferManagement
		{
			public delegate TOutput HandleTransferCompletedDelegate(IntPtr buffer, int actualLength);
			public delegate (IntPtr bufferPtr, int bufferLength) PrepareTransferDelegate(int length);

			public readonly HandleTransferCompletedDelegate HandleTransferCompleted;
			public readonly PrepareTransferDelegate PrepareTransfer;

			public TransferManagement(HandleTransferCompletedDelegate mHandleTransferCompleted, PrepareTransferDelegate mPrepareTransfer)
			{
				HandleTransferCompleted = mHandleTransferCompleted;
				PrepareTransfer = mPrepareTransfer;
			}
		}
	}
}
