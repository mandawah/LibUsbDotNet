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
		private byte[] HandleTransferCompletedDelegate(int offset, int actualLength)
		{
			var bytes = new byte[actualLength];
			Buffer.BlockCopy(m_bytes, offset, bytes, 0, actualLength);
			return bytes;
		}

		private (IntPtr bufferPtr, int bufferLength) PrepareTransferDelegate()
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

		public static TransferManagement<byte[]>[] CreateManagements(int count)
		{
			var managements = new TransferManagement<byte[]>[count];

			for (var i = 0; i < managements.Length; ++i)
			{
				var management = new SimpleByteArrayCopyTransferManagement();
				managements[i] = new TransferManagement<byte[]>(management.PrepareTransferDelegate, management.HandleTransferCompletedDelegate);
			}

			return managements;
		}
	}
}
