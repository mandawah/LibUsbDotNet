using System;
using System.Runtime.InteropServices;

namespace LibUsbDotNet.OsXNativeDelegate
{
	[UnmanagedFunctionPointer(OsXNativeMethods.LibUsbCallingConvention)]
	public delegate int HotplugCallbackFn(Context ctx, Device device, HotplugEvent @event, IntPtr userData);

	[UnmanagedFunctionPointer(OsXNativeMethods.LibUsbCallingConvention)]
	public delegate void PollfdAddedDelegate(int fd, short events, IntPtr userData);

	[UnmanagedFunctionPointer(OsXNativeMethods.LibUsbCallingConvention)]
	public delegate void PollfdRemovedDelegate(int fd, IntPtr userData);

	[UnmanagedFunctionPointer(OsXNativeMethods.LibUsbCallingConvention)]
	public unsafe delegate void TransferDelegate(Transfer* transfer);
}
