﻿using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace LibUsbDotNet.LibUsb
{
	public abstract class AbstractAsyncTransferRunner: IDisposable
	{
		private int m_disposed;
        private int m_started;

        private readonly IntPtr[] m_transfers;
        protected readonly AsyncTransferManagement[] m_managements;

		private readonly UsbContext m_context;
        
        public unsafe AbstractAsyncTransferRunner(UsbDevice usbDevice, byte endPoint, AsyncTransferManagement[] managements)
        {
	        m_context = usbDevice.Context;
	        m_managements = managements;
			
			var parallelTransferCount = managements.Length;
	        m_transfers = new IntPtr[parallelTransferCount];

	        var deviceHandle = usbDevice.DeviceHandle.DangerousGetHandle();
	        
            for (var i = 0; i < m_transfers.Length; ++i)
            {
	            var transfer = CreateTransfer();
	            
	            transfer->DevHandle = deviceHandle;
	            transfer->Endpoint = endPoint;
	            transfer->Timeout = 0;
	            transfer->Flags = (byte)TransferFlags.None;
	            
	            transfer->Callback = Marshal.GetFunctionPointerForDelegate(NativeMethods.TransferDelegate(Callback));
	            
	            transfer->UserData = new IntPtr(i);
                
                m_transfers[i] = new IntPtr(transfer);
            }
        }

        protected abstract unsafe Transfer* CreateTransfer();
        
        public void Start()
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

        protected unsafe void Submit(Transfer* transfer)
        {
	        var transferId = transfer->UserData.ToInt32();
	        
            var transferTask = m_managements[transferId].PrepareTransfer();

            if (transferTask.IsCompleted)
            {
				var (bufferPtr, length) = transferTask.Result;
				Submit(transfer, transferId, bufferPtr, length);
				return;
            }

            transferTask.AsTask().ContinueWith(t =>
            {
	            var (bufferPtr, length) = t.Result;
	            Submit(transfer, transferId, bufferPtr, length);
            }, TaskContinuationOptions.ExecuteSynchronously);
        }

        private unsafe void Submit(Transfer* transfer, int transferId, IntPtr bufferPtr, int length)
        {
	        transfer->Buffer = (byte*)bufferPtr.ToPointer();
	        transfer->Length = length;

	        var status = NativeMethods.SubmitTransfer(transfer);
	        if (status == Error.Success)
	        {
		        return;
	        }

	        m_transfers[transferId] = IntPtr.Zero;
	        NativeMethods.FreeTransfer(transfer);

	        m_managements[transferId].HandleError(status);
        }
		
        private unsafe void Callback(Transfer* transfer)
        {
	        var status = transfer->Status;

	        if (status == TransferStatus.Cancelled)
	        {
		        CancelTransfer(transfer);
		        return;
	        }

            if (status != TransferStatus.Completed)
            {
	            m_managements[transfer->UserData.ToInt32()].HandleError(Error.Other); //TODO Better
                return;
            }
			
            HandleTransferCompleted(transfer);
        }

        protected abstract unsafe void HandleTransferCompleted(Transfer* transfer);

        private unsafe void CancelTransfer(Transfer* transfer)
        {
	        m_transfers[transfer->UserData.ToInt32()] = IntPtr.Zero;
	        NativeMethods.FreeTransfer(transfer);
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
				
				for (var i = 0; i < m_transfers.Length; ++i)
				{
					var transferPtr = m_transfers[i];
					if (transferPtr == IntPtr.Zero)
					{
						continue;
					}

					var transfer = (Transfer*) transferPtr.ToPointer();
					NativeMethods.CancelTransfer(transfer);
					++stillPendingTransfer;
				}

				Thread.Sleep(1);
			}
			while(stillPendingTransfer > 0);

	        m_context.DoesntNeedEventHandlingAnymore();
        }
	}

	public readonly struct AsyncTransferManagement
	{
		public delegate ValueTask<(IntPtr bufferPtr, int bufferLength)> PrepareTransferDelegate();
		public delegate ValueTask HandleTransferCompletedDelegate(int offset, int actualLength);
		public delegate void ErrorHandler(Error error);
			
		public readonly PrepareTransferDelegate PrepareTransfer;
		public readonly HandleTransferCompletedDelegate HandleTransferCompleted;
		public readonly ErrorHandler HandleError;
			
		public AsyncTransferManagement(PrepareTransferDelegate prepareTransfer, HandleTransferCompletedDelegate handleTransferCompleted, ErrorHandler handleError)
		{
			PrepareTransfer = prepareTransfer;
			HandleTransferCompleted = handleTransferCompleted;
			HandleError = handleError;
		}
	}
}