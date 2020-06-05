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

using System;

namespace LibUsbDotNet.LibUsb
{
    /// <summary>
    /// An instance of the libusb API. You can use multiple <see cref="UsbContext"/> which are independent
    /// from each other.
    /// </summary>
    public sealed partial class UsbContext 
    {
        public static readonly UsbContext Default = new UsbContext();
        /// <summary>
        /// The native context.
        /// </summary>
        
		/// <summary>
        /// Initializes a new instance of the <see cref="UsbContext"/> class.
        /// </summary>
        public UsbContext()
        {
            IntPtr contextHandle = IntPtr.Zero;
            NativeMethods.Init(ref contextHandle).ThrowOnError();
            Context = Context.DangerousCreate(contextHandle);
        }

        internal Context Context {get;}

        /// <inheritdoc/>
        public void SetDebugLevel(LogLevel level)
        {
            NativeMethods.SetDebug(Context, (int)level);
        }

        /// <summary>
        /// Returns a list of USB devices currently attached to the system.
        /// </summary>
        /// <returns>
        /// A <see cref="UsbDeviceCollection"/> which contains the devices currently
        /// attached to the system.</returns>
        /// <remarks>
        /// <para>
        /// This is your entry point into finding a USB device to operate.
        /// </para>
        /// <para>
        /// You are expected to dispose all the devices once you are done with them. Disposing the <see cref="UsbDeviceCollection"/>
        /// will dispose all devices in that collection. You can <see cref="UsbDevice.Clone"/> a device to get a copy of the device
        /// which you can use after you've disposed the <see cref="UsbDeviceCollection"/>.
        /// </para>
        /// </remarks>
        public unsafe UsbDevice[] GetDeviceList()
        {
            IntPtr list = IntPtr.Zero;
            var deviceCount = NativeMethods.GetDeviceList(Context, ref list).ToInt32();

            var devices = new UsbDevice[deviceCount];

            var deviceList = (IntPtr*)list.ToPointer();

            for (int i = 0; i < deviceCount; i++)
            {
                var device = Device.DangerousCreate(deviceList[i]);
                
                if (device == null || device == Device.Zero || device.IsClosed || device.IsInvalid)
                {
	                continue;
                }

                devices[i] = new UsbDevice(device, this);
            }

            NativeMethods.FreeDeviceList(list, unrefDevices: 0 /* Do not unreference the devices */);

            return devices;
        }
    }
}
