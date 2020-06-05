using System;
using System.Runtime.InteropServices;
using System.Threading;


namespace Ellisys.Core.Memory
{
	public readonly struct RecyclingBuffer 
	{
		private readonly byte[] m_bytes;
		private readonly int m_offset;
		private readonly object m_parent;

		internal RecyclingBuffer(byte[] bytes, int offset, int count, object parent)
		{
			m_bytes = bytes;
			m_offset = offset;
			Length = count;
			m_parent = parent;
		}

		public int Length { get; }
		public bool IsEmpty => Length == 0;

		
		public ArraySegment<byte> UnsafeGetSegment()
		{
			return new ArraySegment<byte>(m_bytes, m_offset, Length);
		}
	}
}
