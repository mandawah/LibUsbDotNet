using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace Ellisys.Core.Memory
{
	public struct BufferFactory
	{
		public delegate byte[] RentBytesDelegate(int length);
		public delegate void ReturnBytesDelegate(byte[] bytes);

		private object m_currentCargo;
		private IntPtr m_currentCargoPtr;
		private byte[] m_currentCargoBytes;

		private int m_currentCargoAvailableByteCount;

		private int m_currentBufferOffset;
		private int m_currentBufferLength;

		private readonly List<(WeakReference<object> cargoReference, byte[] cargoBytes, GCHandle cargoPinHandle)> m_cargoList;
		
		private readonly RentBytesDelegate m_rent;
		private readonly ReturnBytesDelegate m_return;

		public BufferFactory(RentBytesDelegate rentDelegate, ReturnBytesDelegate returnDelegate)
		{
			m_rent = rentDelegate;
			m_return = returnDelegate;

			m_cargoList	= new List<(WeakReference<object> cargoReference, byte[] cargoBytes, GCHandle cargoPinHandle)>();

			m_currentCargo = null;
			m_currentCargoPtr = IntPtr.Zero;
			m_currentCargoBytes = null;
			
			m_currentCargoAvailableByteCount = 0;

			m_currentBufferOffset = 0;
			m_currentBufferLength = 0;

			AllocateNewBufferIfNeeded(0);
		}

		public (IntPtr buffer, int length) RequestBuffer(int bytesRequested)
		{
			AllocateNewBufferIfNeeded(bytesRequested);

			return (m_currentCargoPtr + m_currentBufferOffset, bytesRequested);
		}

		public RecyclingBuffer ConsumeBuffer(int actualLength)
		{
			var buffer = new RecyclingBuffer(m_currentCargoBytes, m_currentBufferOffset, actualLength, m_currentCargo);

			m_currentBufferOffset += actualLength;
			m_currentCargoAvailableByteCount -= actualLength;

			return buffer;
		}

		private void AllocateNewBufferIfNeeded(int len)
		{
			if (m_currentCargoAvailableByteCount < len)
			{
				AllocateNewCargo(len);
			}
		}

		private void AllocateNewCargo(int len)
		{
			m_currentCargo = null;

			m_currentCargoBytes = RecycleDanglingCargo() ?? m_rent(len);
			var handle = GCHandle.Alloc(m_currentCargoBytes, GCHandleType.Pinned);
			m_currentCargoPtr = handle.AddrOfPinnedObject();

			m_currentCargoAvailableByteCount = m_currentCargoBytes.Length;
			
			m_currentCargo = new object();

			m_currentBufferOffset = 0;

			m_cargoList.Add((new WeakReference<object>(m_currentCargo), m_currentCargoBytes, handle));
		}

		private byte[] RecycleDanglingCargo()
		{
			byte[] reusableBytes = null;

			Span<int> toRemove = stackalloc int[m_cargoList.Count];
			var toRemoveCount = 0;

			for(var index = 0; index < m_cargoList.Count; ++ index)
			{
				var (cargoReference, cargoBytes, cargoPinHandle) = m_cargoList[index]; //could used a fixed list and ref index
				
				if (cargoReference.TryGetTarget(out _))
				{
					continue;
				}

				if (reusableBytes == null)
				{
					reusableBytes = cargoBytes;
#if DEBUG
					//Log.Logger.Debug("Self reused  block #{Id} of size {Size} KBytes", reusableBytes.GetHashCode(), reusableBytes .Length / 1024);
#endif
				}
				else
				{
					m_return(cargoBytes);
					cargoPinHandle.Free();
				}

				toRemove[toRemoveCount++] = index;
			}

			for (var i = 0; i < toRemoveCount; ++i)
			{
				m_cargoList.RemoveAt(toRemove[i]);
			}

			return reusableBytes;
		}
	}
}
