using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using LibUsbDotNet.LibUsb;

namespace LibUbsDotNet.NBS.TransferBufferAllocationManagement
{
	public class SimpleByteArrayCopyTransferManagement
	{ 
		private readonly byte[] m_bytes;
		private readonly int m_bufferSize;
		private GCHandle m_pinHandle;
		private readonly IntPtr m_bytePtr;

		private readonly Func<byte[], ValueTask> m_reception;

		private SimpleByteArrayCopyTransferManagement(Func<byte[], ValueTask> reception, int bufferSize = 65536)
		{
			m_reception = reception;
			m_bufferSize = bufferSize;
			m_bytes = new byte[m_bufferSize];
			m_pinHandle = GCHandle.Alloc(m_bytes, GCHandleType.Pinned);
			m_bytePtr = m_pinHandle.AddrOfPinnedObject();
		}
		private ValueTask HandleTransferCompletedDelegate(int offset, int actualLength)
		{
			var bytes = new byte[actualLength];
			Buffer.BlockCopy(m_bytes, offset, bytes, 0, actualLength);
			return m_reception(bytes);
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

		public static TransferManagement[] CreateManagements(int count, Func<byte[], ValueTask> reception, TransferManagement.ErrorHandler errorHandler)
		{
			var managements = new TransferManagement[count];

			for (var i = 0; i < managements.Length; ++i)
			{
				var management = new SimpleByteArrayCopyTransferManagement(reception);
				managements[i] = new TransferManagement(management.PrepareTransferDelegate, management.HandleTransferCompletedDelegate, errorHandler);
			}

			return managements;
		}
	}
}
