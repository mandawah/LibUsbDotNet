using System;
using System.Runtime.InteropServices;
using System.Threading;
using Ellisys.Core.Collections;
using Ellisys.Core.Logging;

namespace Ellisys.Core.Memory
{
	public readonly struct RecyclingBuffer : IDataVectorContainer
	{
		public delegate int CopyToDelegate(in ReadOnlySpan<byte> span);

		private readonly byte[] m_bytes;
		private readonly int m_offset;
		private readonly RecyclingBufferCargo m_parent;

		internal RecyclingBuffer(byte[] bytes, int offset, int count, RecyclingBufferCargo parent)
		{
			m_bytes = bytes;
			m_offset = offset;
			Length = count;
			m_parent = parent;

#if DEBUG
			if (Length + m_offset > (m_bytes?.Length ?? 0))
			{
				Log.Logger.Error($"RecyclingBuffer is initialized wrongly with: array length= {(m_bytes?.Length ?? 0)}, offset= {m_offset}, Length= {Length}");
			}
#endif
		}

		public int Length { get; }
		public bool IsEmpty => Length == 0;

		internal Memory<byte> InternalMemory => new Memory<byte>(m_bytes, m_offset, Length);

		internal RecyclingBuffer Spawn(int count)
		{
			return m_parent.Spawn(count);
		}

		internal RecyclingBuffer Slice(int start, int length)
		{
#if DEBUG
			if (length + m_offset + start > (m_bytes?.Length ?? 0))
			{
				Log.Logger.Error($"Slice is initialized wrongly with: array length= {(m_bytes?.Length ?? 0)}, offset= {m_offset}, start= {start},  length= {length}");
			}
#endif
			return new RecyclingBuffer(m_bytes, m_offset + start, length, m_parent); 
		}

		public (byte[] data, int offset, int count) UnsafeGetSegment()
		{
			return (m_bytes, m_offset, Length);
		}

		public void CopyFrom(IntPtr bytePtr, int count)
		{
			Log.Logger.Requires(count <= Length, "Cannot copy more byte into buffer than buffer size (wanted to copy {SourceCount}, but buffer size is {BufferSize}", count, Length);
			Marshal.Copy(bytePtr, m_bytes, m_offset, count);
		}

		public void CopyFrom(byte[] source, int sourceOffset, int count)
		{
			Buffer.BlockCopy(source, sourceOffset, m_bytes, m_offset, count);
		}

		public void CopyTo(CopyToDelegate copyToDelegate, CancellationToken token) 
		{
			var offset = m_offset;
			var length = Length;

			while (!token.IsCancellationRequested)
			{
				var span = new ReadOnlySpan<byte>(m_bytes, offset, length);
				var written = copyToDelegate(span);

				offset += written;
				length -= written;

				if (length == 0)
				{
					return;
				}
			}

			token.ThrowIfCancellationRequested();
		}

		public void CopyTo(IntPtr destination, int count)
		{
			Marshal.Copy(m_bytes, m_offset, destination, count);
		}

		public void CopyTo(RecyclingBuffer destination, int destinationOffset, int count)
		{
			Buffer.BlockCopy(m_bytes, m_offset, destination.m_bytes, destination.m_offset + destinationOffset, count);
		}

		public void CopyTo(byte[] destinationBytes, int destinationOffset, int count)
		{
			Buffer.BlockCopy(m_bytes, m_offset, destinationBytes, destinationOffset, count);
		}

		public bool Visit<TVisitor>(int offset, int count, ref TVisitor visitor) where TVisitor : IDataVectorChunkVisitor
		{
			return visitor.ContinueVisit(m_bytes, m_offset + offset, count);
		}

		public bool VisitReversed<TVisitor>(int offset, int count, ref TVisitor visitor) where TVisitor : IDataVectorChunkVisitor
		{
			return Visit(offset, count, ref visitor);
		}
	}
}
