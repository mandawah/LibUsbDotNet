using System;
using System.Runtime.InteropServices;

namespace LibUsbDotNet.LinuxNativeDelegate
{
	[UnmanagedFunctionPointer(LinuxNativeMethods.LibUsbCallingConvention)]
	public delegate int HotplugCallbackFn(Context ctx, Device device, HotplugEvent @event, IntPtr userData);

	[UnmanagedFunctionPointer(LinuxNativeMethods.LibUsbCallingConvention)]
	public delegate void PollfdAddedDelegate(int fd, short events, IntPtr userData);

	[UnmanagedFunctionPointer(LinuxNativeMethods.LibUsbCallingConvention)]
	public delegate void PollfdRemovedDelegate(int fd, IntPtr userData);

	[UnmanagedFunctionPointer(LinuxNativeMethods.LibUsbCallingConvention)]
	public unsafe delegate void TransferDelegate(Transfer* transfer);
}
