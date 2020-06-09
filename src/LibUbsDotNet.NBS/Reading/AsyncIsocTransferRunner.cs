using System.Linq;
using System.Threading.Tasks;
using LibUsbDotNet.Main;

namespace LibUsbDotNet.LibUsb
{
	public class AsyncIsocTransferRunner : AbstractAsyncTransferRunner
	{
		private readonly ValueTask[] m_completedTransferHandlingTasks;
		private readonly int m_numIsoPacketPerTransfer;
		private readonly int m_isoPacketSize;

		public unsafe AsyncIsocTransferRunner(UsbDevice usbDevice, byte endPoint, int isoPacketSize, int numIsoPacketPerTransfer, TransferManagement[] managements)
			: base(usbDevice, endPoint, managements)
        {
	        m_numIsoPacketPerTransfer = numIsoPacketPerTransfer;
	        m_isoPacketSize = isoPacketSize;
	        m_completedTransferHandlingTasks = new ValueTask[numIsoPacketPerTransfer];
        }

		protected override unsafe Transfer* CreateTransfer()
		{
			var transfer = NativeMethods.AllocTransfer(m_numIsoPacketPerTransfer);
			
			transfer->Type = (byte)EndpointType.Isochronous;
			transfer->NumIsoPackets = m_numIsoPacketPerTransfer;
	            
			var descriptors = (IsoPacketDescriptor*)(&transfer->NumIsoPackets + 1);
		            
			for (var p = 0; p < m_numIsoPacketPerTransfer; ++p)
			{
				descriptors[p].Length = (uint)m_isoPacketSize;
			}

			return transfer;
		}

		protected override unsafe void HandleTransfer(Transfer* transfer)
		{
			var transferId = transfer->UserData.ToInt32();

			var descriptors = (IsoPacketDescriptor*) (&transfer->NumIsoPackets + 1);
			
			var offset = 0;
			
			for (var p = 0; p < m_numIsoPacketPerTransfer; ++p)
			{
				var descriptor = descriptors[p];
				if (descriptor.Status == TransferStatus.Completed)
				{
					m_completedTransferHandlingTasks[p] = m_managements[transferId].HandleTransferCompleted(offset, (int) descriptor.ActualLength);
				}
				else
				{
					m_completedTransferHandlingTasks[p] = default;
				}

				offset += (int) descriptor.Length;
			}

			if (m_completedTransferHandlingTasks.All(t => t.IsCompleted))
			{
				Submit(transfer);
				return;
			}

			Task.WhenAll(m_completedTransferHandlingTasks.Select(t => t.AsTask())).ContinueWith(_ => { Submit(transfer); }, TaskContinuationOptions.ExecuteSynchronously);
		}
	}
}
