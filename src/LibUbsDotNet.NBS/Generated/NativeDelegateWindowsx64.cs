using System;
using System.Runtime.InteropServices;

namespace LibUsbDotNet.WindowsNativeDelegatex64
{
	[UnmanagedFunctionPointer(WindowsNativeMethodsx64.LibUsbCallingConvention)]
	public delegate int HotplugCallbackFn(Context ctx, Device device, HotplugEvent @event, IntPtr userData);

	[UnmanagedFunctionPointer(WindowsNativeMethodsx64.LibUsbCallingConvention)]
	public delegate void PollfdAddedDelegate(int fd, short events, IntPtr userData);

	[UnmanagedFunctionPointer(WindowsNativeMethodsx64.LibUsbCallingConvention)]
	public delegate void PollfdRemovedDelegate(int fd, IntPtr userData);

	[UnmanagedFunctionPointer(WindowsNativeMethodsx64.LibUsbCallingConvention)]
	public unsafe delegate void TransferDelegate(Transfer* transfer);
}
