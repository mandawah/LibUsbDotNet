using System;
using System.Runtime.InteropServices;

namespace LibUsbDotNet.WindowsNativeDelegate
{
	[UnmanagedFunctionPointer(WindowsNativeMethods.LibUsbCallingConvention)]
	public delegate int HotplugCallbackFn(Context ctx, Device device, HotplugEvent @event, IntPtr userData);

	[UnmanagedFunctionPointer(WindowsNativeMethods.LibUsbCallingConvention)]
	public delegate void PollfdAddedDelegate(int fd, short events, IntPtr userData);

	[UnmanagedFunctionPointer(WindowsNativeMethods.LibUsbCallingConvention)]
	public delegate void PollfdRemovedDelegate(int fd, IntPtr userData);

	[UnmanagedFunctionPointer(WindowsNativeMethods.LibUsbCallingConvention)]
	public unsafe delegate void TransferDelegate(Transfer* transfer);
}
