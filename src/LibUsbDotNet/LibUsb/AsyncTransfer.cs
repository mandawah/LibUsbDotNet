// Copyright © 2006-2010 Travis Robinson. All rights reserved.
//
// website: http://sourceforge.net/projects/libusbdotnet
// e-mail:  libusbdotnet@gmail.com
//
// This program is free software; you can redistribute it and/or modify it
// under the terms of the GNU General Public License as published by the
// Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful, but
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
// or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
// for more details.
//
// You should have received a copy of the GNU General Public License along
// with this program; if not, write to the Free Software Foundation, Inc.,
// 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA. or
// visit www.gnu.org.
//
//
using LibUsbDotNet.Main;
using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Threading;

namespace LibUsbDotNet.LibUsb
{
    public class AsyncTransfer
    {
        private static readonly ConcurrentDictionary<int, ManualResetEventSlim> Transfers = new ConcurrentDictionary<int, ManualResetEventSlim>();
        private static readonly object TransferLock = new object();
        private static int transferIndex = 0;

        //private static unsafe TransferDelegate transferDelegate = ;
        private unsafe static IntPtr transferDelegatePtr = Marshal.GetFunctionPointerForDelegate(NativeMethods.TransferDelegate(Callback));

        public static unsafe Error TransferAsync(
            DeviceHandle device,
            byte endPoint,
            EndpointType endPointType,
            IntPtr buffer,
            int offset,
            int length,
            int timeout,
            int isoPacketSize,
            out int transferLength)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            // Determin the amount of isosynchronous packets
            int numIsoPackets = 0;

            if (isoPacketSize > 0)
            {
                numIsoPackets = length / isoPacketSize;
            }

            DebugHelper.WriteLine($"IsoSize={isoPacketSize} -> {numIsoPackets}");

            var transfer = NativeMethods.AllocTransfer(numIsoPackets);

            DebugHelper.WriteLine();

            ManualResetEventSlim mre = new ManualResetEventSlim(false);

            DebugHelper.WriteLine();

            int transferId = 0;

            lock (TransferLock)
            {
                transferId = transferIndex++;
            }

            DebugHelper.WriteLine();

            Transfers.AddOrUpdate(transferId, mre, (index, data) => throw new NotImplementedException());
            
			// Fill common properties
            transfer->DevHandle = device.DangerousGetHandle();
            transfer->Endpoint = endPoint;
            transfer->Timeout = (uint)timeout;
            transfer->Type = (byte)endPointType;
            transfer->Buffer = (byte*)buffer + offset;
            transfer->Length = length;
            transfer->NumIsoPackets = numIsoPackets;
            transfer->Flags = (byte)TransferFlags.None;
            transfer->Callback = transferDelegatePtr;
            transfer->UserData = new IntPtr(transferId);

            DebugHelper.WriteLine($"Transfer@: {new IntPtr(transfer)}");
            if (numIsoPackets > 0)
            {
				var descriptors = (IsoPacketDescriptor*)(&transfer->NumIsoPackets + 1);
				DebugHelper.WriteLine($"descriptors@: {new IntPtr(descriptors)}");
				for (var i = 0; i < numIsoPackets; ++i)
				{
					descriptors[i].Length = (uint)isoPacketSize;
					descriptors[i].ActualLength = (uint)1234;
					descriptors[i].Status = TransferStatus.Overflow;
				}

				//NativeMethods.SetIsoPacketLengths(transfer, (uint)isoPacketSize);

    //            var descriptor0 = NativeMethods.GetIsoPacketBuffer(transfer, 0);
				//DebugHelper.WriteLine($"descriptors@: {new IntPtr(descriptor0)}");
				//DebugHelper.WriteLine($"descriptors[0]: {descriptor0->Length}; {descriptor0->ActualLength}; {descriptor0->Status}");
            }

            DebugHelper.WriteLine();
            NativeMethods.SubmitTransfer(transfer).ThrowOnError();
            DebugHelper.WriteLine();
            transferLength = 0;
            mre.Wait();
            DebugHelper.WriteLine();
            transferLength = transfer->ActualLength;
            DebugHelper.WriteLine();
            Error ret = Error.Success;
            switch (transfer->Status)
            {
                case TransferStatus.Completed:
                    ret = Error.Success;
                    break;

                case TransferStatus.TimedOut:
                    ret = Error.Timeout;
                    break;

                case TransferStatus.Stall:
                    ret = Error.Pipe;
                    break;

                case TransferStatus.Overflow:
                    ret = Error.Overflow;
                    break;

                case TransferStatus.NoDevice:
                    ret = Error.NoDevice;
                    break;

                case TransferStatus.Error:
                case TransferStatus.Cancelled:
                    ret = Error.Io;
                    break;

                default:
                    ret = Error.Other;
                    break;
            }
            DebugHelper.WriteLine();
            NativeMethods.FreeTransfer(transfer);
            
            return ret;
        }

        private static unsafe void Callback(Transfer* transfer)
        {
	        var descriptors = (IsoPacketDescriptor*) (&transfer->NumIsoPackets + 1);
	        for (var i = 0; i < 1; ++i)
	        {
		        DebugHelper.WriteLine($"{descriptors[i].Length}; {descriptors[i].ActualLength}; {descriptors[i].Status}");
	        }

	        DebugHelper.WriteLine();
            int id = transfer->UserData.ToInt32();
            if (!Transfers.TryRemove(id, out ManualResetEventSlim transferData))
            {
	            DebugHelper.WriteLine("IGNORING CALLBACK");
            }
            DebugHelper.WriteLine();
            transferData.Set();
            DebugHelper.WriteLine();
        }
    }
}
