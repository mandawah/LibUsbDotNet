using System.Threading.Tasks;
using LibUsbDotNet.Main;

namespace LibUsbDotNet.LibUsb
{
	public class AsyncBulkTransferRunner: AbstractAsyncTransferRunner
	{
		public unsafe AsyncBulkTransferRunner(UsbDevice usbDevice, byte endPoint, AsyncTransferManagement[] managements)
			: base(usbDevice, endPoint, managements)
        {
        }

		protected override unsafe Transfer* CreateTransfer()
		{
			var transfer = NativeMethods.AllocTransfer(0);
			transfer->Type = (byte)EndpointType.Bulk;
			transfer->NumIsoPackets = 0;
			return transfer;
		}

		protected override unsafe void HandleTransferCompleted(Transfer* transfer)
		{
			var transferId = transfer->UserData.ToInt32();

			var transferCompletedHandlingTask = m_managements[transferId].HandleTransferCompleted(0, transfer->ActualLength);

			if (transferCompletedHandlingTask.IsCompleted)
			{
				Submit(transfer);
				return;
			}

			transferCompletedHandlingTask.AsTask().ContinueWith(_ => { Submit(transfer); }, TaskContinuationOptions.ExecuteSynchronously);
		}
	}
}
