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
using System.Threading.Tasks;

namespace LibUsbDotNet.LibUsb
{
    public class AsyncTransfer
    {
        private static readonly ConcurrentDictionary<int, TaskCompletionSource<int>> Transfers = new ConcurrentDictionary<int, TaskCompletionSource<int>>();
        
        private static unsafe TransferDelegate transferDelegate = new TransferDelegate(Callback);
        private static IntPtr transferDelegatePtr = Marshal.GetFunctionPointerForDelegate(transferDelegate);

        private static int TransferId;

		public static async ValueTask<int> TransferAsync(
            DeviceHandle device,
            byte endPoint,
            EndpointType endPointType,
            IntPtr buffer,
            int offset,
            int length,
            int timeout,
            int isoPacketSize,
            CancellationToken cancellationToken = default)
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

			DebugHelper.WriteLine();

            
            Interlocked.Increment(ref TransferId);

            var taskCompletionSource = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously); //use a pool for avoiding allocation? No cannot since one cannot reset the object

            Transfers[TransferId] = taskCompletionSource;

            DebugHelper.WriteLine();

			CancellationTokenRegistration cancelRegister = default;

			try
			{
				unsafe
				{
					var transfer = NativeMethods.AllocTransfer(numIsoPackets);

					DebugHelper.WriteLine();
					//libusb_fill_iso_transfer 
					// Fill common properties
					transfer->DevHandle = device.DangerousGetHandle();
					transfer->Endpoint = endPoint;
					transfer->Timeout =3000;
					transfer->Type = (byte) endPointType;
					transfer->Buffer = (byte*) buffer + offset;
					transfer->Length = length;
					transfer->NumIsoPackets = numIsoPackets;
					transfer->Flags = (byte) TransferFlags.None; //USE FREE TRANSFER AND REMOVE EXPLICIT CALL
					transfer->Callback = transferDelegatePtr;
					transfer->UserData = new IntPtr(TransferId);

					DebugHelper.WriteLine();
					NativeMethods.SubmitTransfer(transfer).ThrowOnError();


					// if(transfer->Status.) Find a way to not rely on semaphore if completed immediately
					DebugHelper.WriteLine();

					cancelRegister = cancellationToken.Register(() =>
					{
						NativeMethods.CancelTransfer(transfer);
					});

					DebugHelper.WriteLine();
				}
				DebugHelper.WriteLine();
				return await taskCompletionSource.Task; //need await becuase of cancellation registration
			}
			finally
			{
				cancelRegister.Dispose();
			}
        }

        private static unsafe void Callback(Transfer* transfer)
        {
	        try
	        {
		        DebugHelper.WriteLine();

		        if (!Transfers.TryRemove(transfer->UserData.ToInt32(), out TaskCompletionSource<int> completionSource))
		        {
			        DebugHelper.WriteLine(transfer->Status.ToString());
					throw new Exception("Completion source not found");
		        }

		        DebugHelper.WriteLine(transfer->Status.ToString());

		        switch (transfer->Status)
		        {
					case TransferStatus.Completed:
						completionSource.SetResult(transfer->ActualLength);
						return;

					case TransferStatus.Cancelled:
						completionSource.SetCanceled();
						return;

			       default:
				        completionSource.SetException(new UsbException(ErrorExtensions.ToError(transfer->Status)));
				        return;
		        }
	        }
	        finally
	        {
		        NativeMethods.FreeTransfer(transfer);
	        }
        }
    }
}
