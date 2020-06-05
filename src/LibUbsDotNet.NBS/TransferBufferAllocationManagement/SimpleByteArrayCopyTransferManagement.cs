using System;
using System.Runtime.InteropServices;
using LibUsbDotNet.LibUsb;

namespace LibUbsDotNet.NBS.TransferBufferAllocationManagement
{
	public class SimpleByteArrayCopyTransferManagement
	{ 
		private readonly byte[] m_bytes;
		private readonly int m_bufferSize;
		private readonly GCHandle m_pinHandle;
		private readonly IntPtr m_bytePtr;

		private SimpleByteArrayCopyTransferManagement(int bufferSize = 65536)
		{
			m_bufferSize = bufferSize;
			m_bytes = new byte[m_bufferSize];
			m_pinHandle = GCHandle.Alloc(m_bytes, GCHandleType.Pinned);
			m_bytePtr = m_pinHandle.AddrOfPinnedObject();
		}
		private byte[] HandleTransferCompletedDelegate(int actualLength)
		{
			var bytes = new byte[actualLength];
			Buffer.BlockCopy(m_bytes, 0, bytes, 0, actualLength);
			return bytes;
		}

		private (IntPtr bufferPtr, int bufferLength) PrepareTransferDelegate(int length)
		{
			return (m_bytePtr, m_bufferSize);
		}

		~SimpleByteArrayCopyTransferManagement()
		{
			try
			{
				m_pinHandle.Free();
			}
			catch
			{
			}
		}

		public static AsyncBulkTransferRunner<byte[]>.TransferManagement[] CreateManagements(int count)
		{
			var managements = new AsyncBulkTransferRunner<byte[]>.TransferManagement[count];

			for (var i = 0; i < managements.Length; ++i)
			{
				var management = new SimpleByteArrayCopyTransferManagement();
				managements[i] = new AsyncBulkTransferRunner<byte[]>.TransferManagement(management.PrepareTransferDelegate, management.HandleTransferCompletedDelegate);
			}

			return managements;
		}
	}
}
