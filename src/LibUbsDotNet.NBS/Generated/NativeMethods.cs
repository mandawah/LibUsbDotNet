using System;
using System.Runtime.InteropServices;
using LibUsbDotNet.LibUsb;

namespace LibUsbDotNet
{ 
	internal unsafe static class NativeMethods
	{
		public delegate Error InitPrototype(ref IntPtr ctx);

        public delegate void ExitPrototype(IntPtr ctx);

        public delegate void SetDebugPrototype(Context ctx, int level);

        public delegate Version* GetVersionPrototype();

        public delegate int HasCapabilityPrototype(uint capability);

        public delegate IntPtr ErrorNamePrototype(int errcode);

        public delegate Error SetLocalePrototype(IntPtr locale);

        public delegate IntPtr StrErrorPrototype(Error errcode);

        public delegate IntPtr GetDeviceListPrototype(Context ctx, ref IntPtr list);

        public delegate void FreeDeviceListPrototype(IntPtr list, int unrefDevices);

        public delegate Device RefDevicePrototype(Device dev);

        public delegate void UnrefDevicePrototype(IntPtr dev);

        public delegate Error GetConfigurationPrototype(DeviceHandle dev, ref int config);

        public delegate Error GetDeviceDescriptorPrototype(Device dev, DeviceDescriptor* desc);

        public delegate Error GetActiveConfigDescriptorPrototype(Device dev, ConfigDescriptor** config);

        public delegate Error GetConfigDescriptorPrototype(Device dev, byte configIndex, ConfigDescriptor** config);

        public delegate Error GetConfigDescriptorByValuePrototype(Device dev, byte bconfigurationvalue, ConfigDescriptor** config);

        public delegate void FreeConfigDescriptorPrototype(ConfigDescriptor* config);

        public delegate Error GetSsEndpointCompanionDescriptorPrototype(ref Context ctx, EndpointDescriptor* endpoint, SsEndpointCompanionDescriptor** epComp);

        public delegate void FreeSsEndpointCompanionDescriptorPrototype(SsEndpointCompanionDescriptor* epComp);

        public delegate Error GetBosDescriptorPrototype(DeviceHandle devHandle, BosDescriptor** bos);

        public delegate void FreeBosDescriptorPrototype(BosDescriptor* bos);

        public delegate Error GetUsb20ExtensionDescriptorPrototype(ref Context ctx, BosDevCapabilityDescriptor* devCap, Usb20ExtensionDescriptor** usb20Extension);

        public delegate void FreeUsb20ExtensionDescriptorPrototype(Usb20ExtensionDescriptor* usb20Extension);

        public delegate Error GetSsUsbDeviceCapabilityDescriptorPrototype(ref Context ctx, BosDevCapabilityDescriptor* devCap, SsUsbDeviceCapabilityDescriptor** ssUsbDeviceCap);

        public delegate void FreeSsUsbDeviceCapabilityDescriptorPrototype(SsUsbDeviceCapabilityDescriptor* ssUsbDeviceCap);

        public delegate Error GetContainerIdDescriptorPrototype(ref Context ctx, BosDevCapabilityDescriptor* devCap, ContainerIdDescriptor** containerId);

        public delegate void FreeContainerIdDescriptorPrototype(ContainerIdDescriptor* containerId);

        public delegate byte GetBusNumberPrototype(Device dev);

        public delegate byte GetPortNumberPrototype(Device dev);

        public delegate Error GetPortNumbersPrototype(Device dev, byte* portNumbers, int portNumbersLen);

        public delegate Error GetPortPathPrototype(Context ctx, Device dev, byte* path, byte pathLength);

        public delegate Device GetParentPrototype(Device dev);

        public delegate byte GetDeviceAddressPrototype(Device dev);

        public delegate int GetDeviceSpeedPrototype(Device dev);

        public delegate int GetMaxPacketSizePrototype(Device dev, byte endpoint);

        public delegate int GetMaxIsoPacketSizePrototype(Device dev, byte endpoint);

        public delegate Error OpenPrototype(Device dev, ref IntPtr devHandle);

        public delegate void ClosePrototype(IntPtr devHandle);

        public delegate Device GetDevicePrototype(DeviceHandle devHandle);

        public delegate Error SetConfigurationPrototype(DeviceHandle devHandle, int configuration);

        public delegate Error ClaimInterfacePrototype(DeviceHandle devHandle, int interfaceNumber);

        public delegate Error ReleaseInterfacePrototype(DeviceHandle devHandle, int interfaceNumber);

        public delegate DeviceHandle OpenDeviceWithVidPidPrototype(Context ctx, ushort vendorId, ushort productId);

        public delegate Error SetInterfaceAltSettingPrototype(DeviceHandle devHandle, int interfaceNumber, int alternateSetting);

        public delegate Error ClearHaltPrototype(DeviceHandle devHandle, byte endpoint);

        public delegate Error ResetDevicePrototype(DeviceHandle devHandle);

        public delegate Error AllocStreamsPrototype(DeviceHandle devHandle, uint numStreams, byte* endpoints, int numEndpoints);

        public delegate Error FreeStreamsPrototype(DeviceHandle devHandle, byte* endpoints, int numEndpoints);

        public delegate byte* DevMemAllocPrototype(DeviceHandle devHandle, UIntPtr length);

        public delegate Error DevMemFreePrototype(DeviceHandle devHandle, byte* buffer, UIntPtr length);

        public delegate int KernelDriverActivePrototype(DeviceHandle devHandle, int interfaceNumber);

        public delegate Error DetachKernelDriverPrototype(DeviceHandle devHandle, int interfaceNumber);

        public delegate Error AttachKernelDriverPrototype(DeviceHandle devHandle, int interfaceNumber);

        public delegate Error SetAutoDetachKernelDriverPrototype(DeviceHandle devHandle, int enable);

        public delegate Transfer* AllocTransferPrototype(int isoPackets);

        public delegate Error SubmitTransferPrototype(Transfer* transfer);

        public delegate Error CancelTransferPrototype(Transfer* transfer);

        public delegate void FreeTransferPrototype(Transfer* transfer);

        public delegate void TransferSetStreamIdPrototype(Transfer* transfer, uint streamId);

        public delegate uint TransferGetStreamIdPrototype(Transfer* transfer);

        public delegate int ControlTransferPrototype(DeviceHandle devHandle, byte requestType, byte brequest, ushort wvalue, ushort windex, byte* data, ushort wlength, uint timeout);

        public delegate Error BulkTransferPrototype(DeviceHandle devHandle, byte endpoint, byte* data, int length, ref int actualLength, uint timeout);

        public delegate Error InterruptTransferPrototype(DeviceHandle devHandle, byte endpoint, byte* data, int length, ref int actualLength, uint timeout);

        public delegate Error GetStringDescriptorAsciiPrototype(DeviceHandle devHandle, byte descIndex, byte* data, int length);

        public delegate int TryLockEventsPrototype(Context ctx);

        public delegate void LockEventsPrototype(Context ctx);

        public delegate void UnlockEventsPrototype(Context ctx);

        public delegate int EventHandlingOkPrototype(Context ctx);

        public delegate int EventHandlerActivePrototype(Context ctx);

        public delegate void InterruptEventHandlerPrototype(Context ctx);

        public delegate void LockEventWaitersPrototype(Context ctx);

        public delegate void UnlockEventWaitersPrototype(Context ctx);

        public delegate int WaitForEventPrototype(Context ctx, ref UnixNativeTimeval tv);

        public delegate Error HandleEventsTimeoutPrototype(Context ctx, ref UnixNativeTimeval tv);

        public delegate Error HandleEventsTimeoutCompletedPrototype(Context ctx, ref UnixNativeTimeval tv, ref int completed);

        public delegate Error HandleEventsPrototype(Context ctx);

        public delegate Error HandleEventsCompletedPrototype(Context ctx, ref int completed);

        public delegate Error HandleEventsLockedPrototype(Context ctx, ref UnixNativeTimeval tv);

        public delegate Error PollfdsHandleTimeoutsPrototype(Context ctx);

        public delegate Error GetNextTimeoutPrototype(Context ctx, ref UnixNativeTimeval tv);

        public delegate Pollfd** GetPollfdsPrototype(Context ctx);

        public delegate void FreePollfdsPrototype(Pollfd** pollfds);

        public delegate void SetPollfdNotifiersPrototype(Context ctx, IntPtr addedDelegate, IntPtr removedDelegate, IntPtr userData);

        public delegate Error HotplugRegisterCallbackPrototype(Context ctx, HotplugEvent events, HotplugFlag flags, int vendorId, int productId, int devClass, IntPtr Delegate, IntPtr userData, ref int callbackHandle);

        public delegate void HotplugDeregisterCallbackPrototype(Context ctx, int callbackHandle);

        //

        public static readonly InitPrototype Init;

        public static readonly ExitPrototype Exit;

        public static readonly SetDebugPrototype SetDebug;

        public static readonly GetVersionPrototype GetVersion;

        public static readonly HasCapabilityPrototype HasCapability;

        public static readonly ErrorNamePrototype ErrorName;

        public static readonly SetLocalePrototype SetLocale;

        public static readonly StrErrorPrototype StrError;

        public static readonly GetDeviceListPrototype GetDeviceList;

        public static readonly FreeDeviceListPrototype FreeDeviceList;

        public static readonly RefDevicePrototype RefDevice;

        public static readonly UnrefDevicePrototype UnrefDevice;

        public static readonly GetConfigurationPrototype GetConfiguration;

        public static readonly GetDeviceDescriptorPrototype GetDeviceDescriptor;

        public static readonly GetActiveConfigDescriptorPrototype GetActiveConfigDescriptor;

        public static readonly GetConfigDescriptorPrototype GetConfigDescriptor;

        public static readonly GetConfigDescriptorByValuePrototype GetConfigDescriptorByValue;

        public static readonly FreeConfigDescriptorPrototype FreeConfigDescriptor;

        public static readonly GetSsEndpointCompanionDescriptorPrototype GetSsEndpointCompanionDescriptor;

        public static readonly FreeSsEndpointCompanionDescriptorPrototype FreeSsEndpointCompanionDescriptor;

        public static readonly GetBosDescriptorPrototype GetBosDescriptor;

        public static readonly FreeBosDescriptorPrototype FreeBosDescriptor;

        public static readonly GetUsb20ExtensionDescriptorPrototype GetUsb20ExtensionDescriptor;

        public static readonly FreeUsb20ExtensionDescriptorPrototype FreeUsb20ExtensionDescriptor;

        public static readonly GetSsUsbDeviceCapabilityDescriptorPrototype GetSsUsbDeviceCapabilityDescriptor;

        public static readonly FreeSsUsbDeviceCapabilityDescriptorPrototype FreeSsUsbDeviceCapabilityDescriptor;

        public static readonly GetContainerIdDescriptorPrototype GetContainerIdDescriptor;

        public static readonly FreeContainerIdDescriptorPrototype FreeContainerIdDescriptor;

        public static readonly GetBusNumberPrototype GetBusNumber;

        public static readonly GetPortNumberPrototype GetPortNumber;

        public static readonly GetPortNumbersPrototype GetPortNumbers;

        public static readonly GetPortPathPrototype GetPortPath;

        public static readonly GetParentPrototype GetParent;

        public static readonly GetDeviceAddressPrototype GetDeviceAddress;

        public static readonly GetDeviceSpeedPrototype GetDeviceSpeed;

        public static readonly GetMaxPacketSizePrototype GetMaxPacketSize;

        public static readonly GetMaxIsoPacketSizePrototype GetMaxIsoPacketSize;

        public static readonly OpenPrototype Open;

        public static readonly ClosePrototype Close;

        public static readonly GetDevicePrototype GetDevice;

        public static readonly SetConfigurationPrototype SetConfiguration;

        public static readonly ClaimInterfacePrototype ClaimInterface;

        public static readonly ReleaseInterfacePrototype ReleaseInterface;

        public static readonly OpenDeviceWithVidPidPrototype OpenDeviceWithVidPid;

        public static readonly SetInterfaceAltSettingPrototype SetInterfaceAltSetting;

        public static readonly ClearHaltPrototype ClearHalt;

        public static readonly ResetDevicePrototype ResetDevice;

        public static readonly AllocStreamsPrototype AllocStreams;

        public static readonly FreeStreamsPrototype FreeStreams;

        public static readonly DevMemAllocPrototype DevMemAlloc;

        public static readonly DevMemFreePrototype DevMemFree;

        public static readonly KernelDriverActivePrototype KernelDriverActive;

        public static readonly DetachKernelDriverPrototype DetachKernelDriver;

        public static readonly AttachKernelDriverPrototype AttachKernelDriver;

        public static readonly SetAutoDetachKernelDriverPrototype SetAutoDetachKernelDriver;

        public static readonly AllocTransferPrototype AllocTransfer;

        public static readonly SubmitTransferPrototype SubmitTransfer;

        public static readonly CancelTransferPrototype CancelTransfer;

        public static readonly FreeTransferPrototype FreeTransfer;

        public static readonly TransferSetStreamIdPrototype TransferSetStreamId;

        public static readonly TransferGetStreamIdPrototype TransferGetStreamId;

        public static readonly ControlTransferPrototype ControlTransfer;

        public static readonly BulkTransferPrototype BulkTransfer;

        public static readonly InterruptTransferPrototype InterruptTransfer;

        public static readonly GetStringDescriptorAsciiPrototype GetStringDescriptorAscii;

        public static readonly TryLockEventsPrototype TryLockEvents;

        public static readonly LockEventsPrototype LockEvents;

        public static readonly UnlockEventsPrototype UnlockEvents;

        public static readonly EventHandlingOkPrototype EventHandlingOk;

        public static readonly EventHandlerActivePrototype EventHandlerActive;

        public static readonly InterruptEventHandlerPrototype InterruptEventHandler;

        public static readonly LockEventWaitersPrototype LockEventWaiters;

        public static readonly UnlockEventWaitersPrototype UnlockEventWaiters;

        public static readonly WaitForEventPrototype WaitForEvent;

        public static readonly HandleEventsTimeoutPrototype HandleEventsTimeout;

        public static readonly HandleEventsTimeoutCompletedPrototype HandleEventsTimeoutCompleted;

        public static readonly HandleEventsPrototype HandleEvents;

        public static readonly HandleEventsCompletedPrototype HandleEventsCompleted;

        public static readonly HandleEventsLockedPrototype HandleEventsLocked;

        public static readonly PollfdsHandleTimeoutsPrototype PollfdsHandleTimeouts;

        public static readonly GetNextTimeoutPrototype GetNextTimeout;

        public static readonly GetPollfdsPrototype GetPollfds;

        public static readonly FreePollfdsPrototype FreePollfds;

        public static readonly SetPollfdNotifiersPrototype SetPollfdNotifiers;

        public static readonly HotplugRegisterCallbackPrototype HotplugRegisterCallback;

        public static readonly HotplugDeregisterCallbackPrototype HotplugDeregisterCallback;

        public const int Pack  = 0; // To validate: does it really works like this?

        public delegate void AgnosticTransferDelegate(Transfer* transfer);
        public static readonly Func<AgnosticTransferDelegate, Delegate> TransferDelegate;

        public delegate int AgnosticHotplugCallbackFn(Context ctx, Device device, HotplugEvent @event, IntPtr userData);
        public static readonly Func<AgnosticHotplugCallbackFn, Delegate> HotplugCallbackFn;

        public delegate void AgnosticPollfdAddedDelegate(int fd, short events, IntPtr userData);
        public static readonly Func<AgnosticPollfdAddedDelegate, Delegate> PollfdAddedDelegate;

        public delegate void AgnosticPollfdRemovedDelegate(int fd, IntPtr userData);
        public static readonly Func<AgnosticPollfdRemovedDelegate, Delegate> PollfdRemovedDelegate;

        static NativeMethods()
        {
	        switch (Environment.OSVersion.Platform)
	        {
		        case PlatformID.Unix:

			        TransferDelegate = d => new LinuxNativeDelegate.TransferDelegate(d);
			        HotplugCallbackFn = d => new LinuxNativeDelegate.HotplugCallbackFn(d);
			        PollfdAddedDelegate = d => new LinuxNativeDelegate.PollfdAddedDelegate(d);
			        PollfdRemovedDelegate = d => new LinuxNativeDelegate.PollfdRemovedDelegate(d);

			        Init = LinuxNativeMethods.Init;

			        Exit = LinuxNativeMethods.Exit;

			        SetDebug = LinuxNativeMethods.SetDebug;

			        GetVersion = LinuxNativeMethods.GetVersion;

			        HasCapability = LinuxNativeMethods.HasCapability;

			        ErrorName = LinuxNativeMethods.ErrorName;

			        SetLocale = LinuxNativeMethods.SetLocale;

			        StrError = LinuxNativeMethods.StrError;

			        GetDeviceList = LinuxNativeMethods.GetDeviceList;

			        FreeDeviceList = LinuxNativeMethods.FreeDeviceList;

			        RefDevice = LinuxNativeMethods.RefDevice;

			        UnrefDevice = LinuxNativeMethods.UnrefDevice;

			        GetConfiguration = LinuxNativeMethods.GetConfiguration;

			        GetDeviceDescriptor = LinuxNativeMethods.GetDeviceDescriptor;

			        GetActiveConfigDescriptor = LinuxNativeMethods.GetActiveConfigDescriptor;

			        GetConfigDescriptor = LinuxNativeMethods.GetConfigDescriptor;

			        GetConfigDescriptorByValue = LinuxNativeMethods.GetConfigDescriptorByValue;

			        FreeConfigDescriptor = LinuxNativeMethods.FreeConfigDescriptor;

			        GetSsEndpointCompanionDescriptor = LinuxNativeMethods.GetSsEndpointCompanionDescriptor;

			        FreeSsEndpointCompanionDescriptor = LinuxNativeMethods.FreeSsEndpointCompanionDescriptor;

			        GetBosDescriptor = LinuxNativeMethods.GetBosDescriptor;

			        FreeBosDescriptor = LinuxNativeMethods.FreeBosDescriptor;

			        GetUsb20ExtensionDescriptor = LinuxNativeMethods.GetUsb20ExtensionDescriptor;

			        FreeUsb20ExtensionDescriptor = LinuxNativeMethods.FreeUsb20ExtensionDescriptor;

			        GetSsUsbDeviceCapabilityDescriptor = LinuxNativeMethods.GetSsUsbDeviceCapabilityDescriptor;

			        FreeSsUsbDeviceCapabilityDescriptor = LinuxNativeMethods.FreeSsUsbDeviceCapabilityDescriptor;

			        GetContainerIdDescriptor = LinuxNativeMethods.GetContainerIdDescriptor;

			        FreeContainerIdDescriptor = LinuxNativeMethods.FreeContainerIdDescriptor;

			        GetBusNumber = LinuxNativeMethods.GetBusNumber;

			        GetPortNumber = LinuxNativeMethods.GetPortNumber;

			        GetPortNumbers = LinuxNativeMethods.GetPortNumbers;

			        GetPortPath = LinuxNativeMethods.GetPortPath;

			        GetParent = LinuxNativeMethods.GetParent;

			        GetDeviceAddress = LinuxNativeMethods.GetDeviceAddress;

			        GetDeviceSpeed = LinuxNativeMethods.GetDeviceSpeed;

			        GetMaxPacketSize = LinuxNativeMethods.GetMaxPacketSize;

			        GetMaxIsoPacketSize = LinuxNativeMethods.GetMaxIsoPacketSize;

			        Open = LinuxNativeMethods.Open;

			        Close = LinuxNativeMethods.Close;

			        GetDevice = LinuxNativeMethods.GetDevice;

			        SetConfiguration = LinuxNativeMethods.SetConfiguration;

			        ClaimInterface = LinuxNativeMethods.ClaimInterface;

			        ReleaseInterface = LinuxNativeMethods.ReleaseInterface;

			        OpenDeviceWithVidPid = LinuxNativeMethods.OpenDeviceWithVidPid;

			        SetInterfaceAltSetting = LinuxNativeMethods.SetInterfaceAltSetting;

			        ClearHalt = LinuxNativeMethods.ClearHalt;

			        ResetDevice = LinuxNativeMethods.ResetDevice;

			        AllocStreams = LinuxNativeMethods.AllocStreams;

			        FreeStreams = LinuxNativeMethods.FreeStreams;

			        DevMemAlloc = LinuxNativeMethods.DevMemAlloc;

			        DevMemFree = LinuxNativeMethods.DevMemFree;

			        KernelDriverActive = LinuxNativeMethods.KernelDriverActive;

			        DetachKernelDriver = LinuxNativeMethods.DetachKernelDriver;

			        AttachKernelDriver = LinuxNativeMethods.AttachKernelDriver;

			        SetAutoDetachKernelDriver = LinuxNativeMethods.SetAutoDetachKernelDriver;

			        AllocTransfer = LinuxNativeMethods.AllocTransfer;

			        SubmitTransfer = LinuxNativeMethods.SubmitTransfer;

			        CancelTransfer = LinuxNativeMethods.CancelTransfer;

			        FreeTransfer = LinuxNativeMethods.FreeTransfer;

			        TransferSetStreamId = LinuxNativeMethods.TransferSetStreamId;

			        TransferGetStreamId = LinuxNativeMethods.TransferGetStreamId;

			        ControlTransfer = LinuxNativeMethods.ControlTransfer;

			        BulkTransfer = LinuxNativeMethods.BulkTransfer;

			        InterruptTransfer = LinuxNativeMethods.InterruptTransfer;

			        GetStringDescriptorAscii = LinuxNativeMethods.GetStringDescriptorAscii;

			        TryLockEvents = LinuxNativeMethods.TryLockEvents;

			        LockEvents = LinuxNativeMethods.LockEvents;

			        UnlockEvents = LinuxNativeMethods.UnlockEvents;

			        EventHandlingOk = LinuxNativeMethods.EventHandlingOk;

			        EventHandlerActive = LinuxNativeMethods.EventHandlerActive;

			        InterruptEventHandler = LinuxNativeMethods.InterruptEventHandler;

			        LockEventWaiters = LinuxNativeMethods.LockEventWaiters;

			        UnlockEventWaiters = LinuxNativeMethods.UnlockEventWaiters;

			        WaitForEvent = LinuxNativeMethods.WaitForEvent;

			        HandleEventsTimeout = LinuxNativeMethods.HandleEventsTimeout;

			        HandleEventsTimeoutCompleted = LinuxNativeMethods.HandleEventsTimeoutCompleted;

			        HandleEvents = LinuxNativeMethods.HandleEvents;

			        HandleEventsCompleted = LinuxNativeMethods.HandleEventsCompleted;

			        HandleEventsLocked = LinuxNativeMethods.HandleEventsLocked;

			        PollfdsHandleTimeouts = LinuxNativeMethods.PollfdsHandleTimeouts;

			        GetNextTimeout = LinuxNativeMethods.GetNextTimeout;

			        GetPollfds = LinuxNativeMethods.GetPollfds;

			        FreePollfds = LinuxNativeMethods.FreePollfds;

			        SetPollfdNotifiers = LinuxNativeMethods.SetPollfdNotifiers;

			        HotplugRegisterCallback = LinuxNativeMethods.HotplugRegisterCallback;

			        HotplugDeregisterCallback = LinuxNativeMethods.HotplugDeregisterCallback;

			        break;


                 case PlatformID.MacOSX:
	                 
					TransferDelegate = d => new OsXNativeDelegate.TransferDelegate(d);
	                HotplugCallbackFn = d => new OsXNativeDelegate.HotplugCallbackFn(d);
	                PollfdAddedDelegate = d => new OsXNativeDelegate.PollfdAddedDelegate(d);
	                PollfdRemovedDelegate = d => new OsXNativeDelegate.PollfdRemovedDelegate(d);

			        Init = OsXNativeMethods.Init;

			        Exit = OsXNativeMethods.Exit;

			        SetDebug = OsXNativeMethods.SetDebug;

			        GetVersion = OsXNativeMethods.GetVersion;

			        HasCapability = OsXNativeMethods.HasCapability;

			        ErrorName = OsXNativeMethods.ErrorName;

			        SetLocale = OsXNativeMethods.SetLocale;

			        StrError = OsXNativeMethods.StrError;

			        GetDeviceList = OsXNativeMethods.GetDeviceList;

			        FreeDeviceList = OsXNativeMethods.FreeDeviceList;

			        RefDevice = OsXNativeMethods.RefDevice;

			        UnrefDevice = OsXNativeMethods.UnrefDevice;

			        GetConfiguration = OsXNativeMethods.GetConfiguration;

			        GetDeviceDescriptor = OsXNativeMethods.GetDeviceDescriptor;

			        GetActiveConfigDescriptor = OsXNativeMethods.GetActiveConfigDescriptor;

			        GetConfigDescriptor = OsXNativeMethods.GetConfigDescriptor;

			        GetConfigDescriptorByValue = OsXNativeMethods.GetConfigDescriptorByValue;

			        FreeConfigDescriptor = OsXNativeMethods.FreeConfigDescriptor;

			        GetSsEndpointCompanionDescriptor = OsXNativeMethods.GetSsEndpointCompanionDescriptor;

			        FreeSsEndpointCompanionDescriptor = OsXNativeMethods.FreeSsEndpointCompanionDescriptor;

			        GetBosDescriptor = OsXNativeMethods.GetBosDescriptor;

			        FreeBosDescriptor = OsXNativeMethods.FreeBosDescriptor;

			        GetUsb20ExtensionDescriptor = OsXNativeMethods.GetUsb20ExtensionDescriptor;

			        FreeUsb20ExtensionDescriptor = OsXNativeMethods.FreeUsb20ExtensionDescriptor;

			        GetSsUsbDeviceCapabilityDescriptor = OsXNativeMethods.GetSsUsbDeviceCapabilityDescriptor;

			        FreeSsUsbDeviceCapabilityDescriptor = OsXNativeMethods.FreeSsUsbDeviceCapabilityDescriptor;

			        GetContainerIdDescriptor = OsXNativeMethods.GetContainerIdDescriptor;

			        FreeContainerIdDescriptor = OsXNativeMethods.FreeContainerIdDescriptor;

			        GetBusNumber = OsXNativeMethods.GetBusNumber;

			        GetPortNumber = OsXNativeMethods.GetPortNumber;

			        GetPortNumbers = OsXNativeMethods.GetPortNumbers;

			        GetPortPath = OsXNativeMethods.GetPortPath;

			        GetParent = OsXNativeMethods.GetParent;

			        GetDeviceAddress = OsXNativeMethods.GetDeviceAddress;

			        GetDeviceSpeed = OsXNativeMethods.GetDeviceSpeed;

			        GetMaxPacketSize = OsXNativeMethods.GetMaxPacketSize;

			        GetMaxIsoPacketSize = OsXNativeMethods.GetMaxIsoPacketSize;

			        Open = OsXNativeMethods.Open;

			        Close = OsXNativeMethods.Close;

			        GetDevice = OsXNativeMethods.GetDevice;

			        SetConfiguration = OsXNativeMethods.SetConfiguration;

			        ClaimInterface = OsXNativeMethods.ClaimInterface;

			        ReleaseInterface = OsXNativeMethods.ReleaseInterface;

			        OpenDeviceWithVidPid = OsXNativeMethods.OpenDeviceWithVidPid;

			        SetInterfaceAltSetting = OsXNativeMethods.SetInterfaceAltSetting;

			        ClearHalt = OsXNativeMethods.ClearHalt;

			        ResetDevice = OsXNativeMethods.ResetDevice;

			        AllocStreams = OsXNativeMethods.AllocStreams;

			        FreeStreams = OsXNativeMethods.FreeStreams;

			        DevMemAlloc = OsXNativeMethods.DevMemAlloc;

			        DevMemFree = OsXNativeMethods.DevMemFree;

			        KernelDriverActive = OsXNativeMethods.KernelDriverActive;

			        DetachKernelDriver = OsXNativeMethods.DetachKernelDriver;

			        AttachKernelDriver = OsXNativeMethods.AttachKernelDriver;

			        SetAutoDetachKernelDriver = OsXNativeMethods.SetAutoDetachKernelDriver;

			        AllocTransfer = OsXNativeMethods.AllocTransfer;

			        SubmitTransfer = OsXNativeMethods.SubmitTransfer;

			        CancelTransfer = OsXNativeMethods.CancelTransfer;

			        FreeTransfer = OsXNativeMethods.FreeTransfer;

			        TransferSetStreamId = OsXNativeMethods.TransferSetStreamId;

			        TransferGetStreamId = OsXNativeMethods.TransferGetStreamId;

			        ControlTransfer = OsXNativeMethods.ControlTransfer;

			        BulkTransfer = OsXNativeMethods.BulkTransfer;

			        InterruptTransfer = OsXNativeMethods.InterruptTransfer;

			        GetStringDescriptorAscii = OsXNativeMethods.GetStringDescriptorAscii;

			        TryLockEvents = OsXNativeMethods.TryLockEvents;

			        LockEvents = OsXNativeMethods.LockEvents;

			        UnlockEvents = OsXNativeMethods.UnlockEvents;

			        EventHandlingOk = OsXNativeMethods.EventHandlingOk;

			        EventHandlerActive = OsXNativeMethods.EventHandlerActive;

			        InterruptEventHandler = OsXNativeMethods.InterruptEventHandler;

			        LockEventWaiters = OsXNativeMethods.LockEventWaiters;

			        UnlockEventWaiters = OsXNativeMethods.UnlockEventWaiters;

			        WaitForEvent = OsXNativeMethods.WaitForEvent;

			        HandleEventsTimeout = OsXNativeMethods.HandleEventsTimeout;

			        HandleEventsTimeoutCompleted = OsXNativeMethods.HandleEventsTimeoutCompleted;

			        HandleEvents = OsXNativeMethods.HandleEvents;

			        HandleEventsCompleted = OsXNativeMethods.HandleEventsCompleted;

			        HandleEventsLocked = OsXNativeMethods.HandleEventsLocked;

			        PollfdsHandleTimeouts = OsXNativeMethods.PollfdsHandleTimeouts;

			        GetNextTimeout = OsXNativeMethods.GetNextTimeout;

			        GetPollfds = OsXNativeMethods.GetPollfds;

			        FreePollfds = OsXNativeMethods.FreePollfds;

			        SetPollfdNotifiers = OsXNativeMethods.SetPollfdNotifiers;

			        HotplugRegisterCallback = OsXNativeMethods.HotplugRegisterCallback;

			        HotplugDeregisterCallback = OsXNativeMethods.HotplugDeregisterCallback;

			        break;


				 default:

					 if (!Environment.Is64BitProcess)
					 {
						 TransferDelegate = d => new WindowsNativeDelegatex86.TransferDelegate(d);
						 HotplugCallbackFn = d => new WindowsNativeDelegatex86.HotplugCallbackFn(d);
						 PollfdAddedDelegate = d => new WindowsNativeDelegatex86.PollfdAddedDelegate(d);
						 PollfdRemovedDelegate = d => new WindowsNativeDelegatex86.PollfdRemovedDelegate(d);

						 Init = WindowsNativeMethodsx86.Init;

						 Exit = WindowsNativeMethodsx86.Exit;

						 SetDebug = WindowsNativeMethodsx86.SetDebug;

						 GetVersion = WindowsNativeMethodsx86.GetVersion;

						 HasCapability = WindowsNativeMethodsx86.HasCapability;

						 ErrorName = WindowsNativeMethodsx86.ErrorName;

						 SetLocale = WindowsNativeMethodsx86.SetLocale;

						 StrError = WindowsNativeMethodsx86.StrError;

						 GetDeviceList = WindowsNativeMethodsx86.GetDeviceList;

						 FreeDeviceList = WindowsNativeMethodsx86.FreeDeviceList;

						 RefDevice = WindowsNativeMethodsx86.RefDevice;

						 UnrefDevice = WindowsNativeMethodsx86.UnrefDevice;

						 GetConfiguration = WindowsNativeMethodsx86.GetConfiguration;

						 GetDeviceDescriptor = WindowsNativeMethodsx86.GetDeviceDescriptor;

						 GetActiveConfigDescriptor = WindowsNativeMethodsx86.GetActiveConfigDescriptor;

						 GetConfigDescriptor = WindowsNativeMethodsx86.GetConfigDescriptor;

						 GetConfigDescriptorByValue = WindowsNativeMethodsx86.GetConfigDescriptorByValue;

						 FreeConfigDescriptor = WindowsNativeMethodsx86.FreeConfigDescriptor;

						 GetSsEndpointCompanionDescriptor = WindowsNativeMethodsx86.GetSsEndpointCompanionDescriptor;

						 FreeSsEndpointCompanionDescriptor = WindowsNativeMethodsx86.FreeSsEndpointCompanionDescriptor;

						 GetBosDescriptor = WindowsNativeMethodsx86.GetBosDescriptor;

						 FreeBosDescriptor = WindowsNativeMethodsx86.FreeBosDescriptor;

						 GetUsb20ExtensionDescriptor = WindowsNativeMethodsx86.GetUsb20ExtensionDescriptor;

						 FreeUsb20ExtensionDescriptor = WindowsNativeMethodsx86.FreeUsb20ExtensionDescriptor;

						 GetSsUsbDeviceCapabilityDescriptor = WindowsNativeMethodsx86.GetSsUsbDeviceCapabilityDescriptor;

						 FreeSsUsbDeviceCapabilityDescriptor = WindowsNativeMethodsx86.FreeSsUsbDeviceCapabilityDescriptor;

						 GetContainerIdDescriptor = WindowsNativeMethodsx86.GetContainerIdDescriptor;

						 FreeContainerIdDescriptor = WindowsNativeMethodsx86.FreeContainerIdDescriptor;

						 GetBusNumber = WindowsNativeMethodsx86.GetBusNumber;

						 GetPortNumber = WindowsNativeMethodsx86.GetPortNumber;

						 GetPortNumbers = WindowsNativeMethodsx86.GetPortNumbers;

						 GetPortPath = WindowsNativeMethodsx86.GetPortPath;

						 GetParent = WindowsNativeMethodsx86.GetParent;

						 GetDeviceAddress = WindowsNativeMethodsx86.GetDeviceAddress;

						 GetDeviceSpeed = WindowsNativeMethodsx86.GetDeviceSpeed;

						 GetMaxPacketSize = WindowsNativeMethodsx86.GetMaxPacketSize;

						 GetMaxIsoPacketSize = WindowsNativeMethodsx86.GetMaxIsoPacketSize;

						 Open = WindowsNativeMethodsx86.Open;

						 Close = WindowsNativeMethodsx86.Close;

						 GetDevice = WindowsNativeMethodsx86.GetDevice;

						 SetConfiguration = WindowsNativeMethodsx86.SetConfiguration;

						 ClaimInterface = WindowsNativeMethodsx86.ClaimInterface;

						 ReleaseInterface = WindowsNativeMethodsx86.ReleaseInterface;

						 OpenDeviceWithVidPid = WindowsNativeMethodsx86.OpenDeviceWithVidPid;

						 SetInterfaceAltSetting = WindowsNativeMethodsx86.SetInterfaceAltSetting;

						 ClearHalt = WindowsNativeMethodsx86.ClearHalt;

						 ResetDevice = WindowsNativeMethodsx86.ResetDevice;

						 AllocStreams = WindowsNativeMethodsx86.AllocStreams;

						 FreeStreams = WindowsNativeMethodsx86.FreeStreams;

						 DevMemAlloc = WindowsNativeMethodsx86.DevMemAlloc;

						 DevMemFree = WindowsNativeMethodsx86.DevMemFree;

						 KernelDriverActive = WindowsNativeMethodsx86.KernelDriverActive;

						 DetachKernelDriver = WindowsNativeMethodsx86.DetachKernelDriver;

						 AttachKernelDriver = WindowsNativeMethodsx86.AttachKernelDriver;

						 SetAutoDetachKernelDriver = WindowsNativeMethodsx86.SetAutoDetachKernelDriver;

						 AllocTransfer = WindowsNativeMethodsx86.AllocTransfer;

						 SubmitTransfer = WindowsNativeMethodsx86.SubmitTransfer;

						 CancelTransfer = WindowsNativeMethodsx86.CancelTransfer;

						 FreeTransfer = WindowsNativeMethodsx86.FreeTransfer;

						 TransferSetStreamId = WindowsNativeMethodsx86.TransferSetStreamId;

						 TransferGetStreamId = WindowsNativeMethodsx86.TransferGetStreamId;

						 ControlTransfer = WindowsNativeMethodsx86.ControlTransfer;

						 BulkTransfer = WindowsNativeMethodsx86.BulkTransfer;

						 InterruptTransfer = WindowsNativeMethodsx86.InterruptTransfer;

						 GetStringDescriptorAscii = WindowsNativeMethodsx86.GetStringDescriptorAscii;

						 TryLockEvents = WindowsNativeMethodsx86.TryLockEvents;

						 LockEvents = WindowsNativeMethodsx86.LockEvents;

						 UnlockEvents = WindowsNativeMethodsx86.UnlockEvents;

						 EventHandlingOk = WindowsNativeMethodsx86.EventHandlingOk;

						 EventHandlerActive = WindowsNativeMethodsx86.EventHandlerActive;

						 InterruptEventHandler = WindowsNativeMethodsx86.InterruptEventHandler;

						 LockEventWaiters = WindowsNativeMethodsx86.LockEventWaiters;

						 UnlockEventWaiters = WindowsNativeMethodsx86.UnlockEventWaiters;

						 WaitForEvent = WindowsNativeMethodsx86.WaitForEvent;

						 HandleEventsTimeout = WindowsNativeMethodsx86.HandleEventsTimeout;

						 HandleEventsTimeoutCompleted = WindowsNativeMethodsx86.HandleEventsTimeoutCompleted;

						 HandleEvents = WindowsNativeMethodsx86.HandleEvents;

						 HandleEventsCompleted = WindowsNativeMethodsx86.HandleEventsCompleted;

						 HandleEventsLocked = WindowsNativeMethodsx86.HandleEventsLocked;

						 PollfdsHandleTimeouts = WindowsNativeMethodsx86.PollfdsHandleTimeouts;

						 GetNextTimeout = WindowsNativeMethodsx86.GetNextTimeout;

						 GetPollfds = WindowsNativeMethodsx86.GetPollfds;

						 FreePollfds = WindowsNativeMethodsx86.FreePollfds;

						 SetPollfdNotifiers = WindowsNativeMethodsx86.SetPollfdNotifiers;

						 HotplugRegisterCallback = WindowsNativeMethodsx86.HotplugRegisterCallback;

						 HotplugDeregisterCallback = WindowsNativeMethodsx86.HotplugDeregisterCallback;
					 }
					 else
					 {
						 TransferDelegate = d => new WindowsNativeDelegatex64.TransferDelegate(d);
						 HotplugCallbackFn = d => new WindowsNativeDelegatex64.HotplugCallbackFn(d);
						 PollfdAddedDelegate = d => new WindowsNativeDelegatex64.PollfdAddedDelegate(d);
						 PollfdRemovedDelegate = d => new WindowsNativeDelegatex64.PollfdRemovedDelegate(d);

						 Init = WindowsNativeMethodsx64.Init;

						 Exit = WindowsNativeMethodsx64.Exit;

						 SetDebug = WindowsNativeMethodsx64.SetDebug;

						 GetVersion = WindowsNativeMethodsx64.GetVersion;

						 HasCapability = WindowsNativeMethodsx64.HasCapability;

						 ErrorName = WindowsNativeMethodsx64.ErrorName;

						 SetLocale = WindowsNativeMethodsx64.SetLocale;

						 StrError = WindowsNativeMethodsx64.StrError;

						 GetDeviceList = WindowsNativeMethodsx64.GetDeviceList;

						 FreeDeviceList = WindowsNativeMethodsx64.FreeDeviceList;

						 RefDevice = WindowsNativeMethodsx64.RefDevice;

						 UnrefDevice = WindowsNativeMethodsx64.UnrefDevice;

						 GetConfiguration = WindowsNativeMethodsx64.GetConfiguration;

						 GetDeviceDescriptor = WindowsNativeMethodsx64.GetDeviceDescriptor;

						 GetActiveConfigDescriptor = WindowsNativeMethodsx64.GetActiveConfigDescriptor;

						 GetConfigDescriptor = WindowsNativeMethodsx64.GetConfigDescriptor;

						 GetConfigDescriptorByValue = WindowsNativeMethodsx64.GetConfigDescriptorByValue;

						 FreeConfigDescriptor = WindowsNativeMethodsx64.FreeConfigDescriptor;

						 GetSsEndpointCompanionDescriptor = WindowsNativeMethodsx64.GetSsEndpointCompanionDescriptor;

						 FreeSsEndpointCompanionDescriptor = WindowsNativeMethodsx64.FreeSsEndpointCompanionDescriptor;

						 GetBosDescriptor = WindowsNativeMethodsx64.GetBosDescriptor;

						 FreeBosDescriptor = WindowsNativeMethodsx64.FreeBosDescriptor;

						 GetUsb20ExtensionDescriptor = WindowsNativeMethodsx64.GetUsb20ExtensionDescriptor;

						 FreeUsb20ExtensionDescriptor = WindowsNativeMethodsx64.FreeUsb20ExtensionDescriptor;

						 GetSsUsbDeviceCapabilityDescriptor = WindowsNativeMethodsx64.GetSsUsbDeviceCapabilityDescriptor;

						 FreeSsUsbDeviceCapabilityDescriptor = WindowsNativeMethodsx64.FreeSsUsbDeviceCapabilityDescriptor;

						 GetContainerIdDescriptor = WindowsNativeMethodsx64.GetContainerIdDescriptor;

						 FreeContainerIdDescriptor = WindowsNativeMethodsx64.FreeContainerIdDescriptor;

						 GetBusNumber = WindowsNativeMethodsx64.GetBusNumber;

						 GetPortNumber = WindowsNativeMethodsx64.GetPortNumber;

						 GetPortNumbers = WindowsNativeMethodsx64.GetPortNumbers;

						 GetPortPath = WindowsNativeMethodsx64.GetPortPath;

						 GetParent = WindowsNativeMethodsx64.GetParent;

						 GetDeviceAddress = WindowsNativeMethodsx64.GetDeviceAddress;

						 GetDeviceSpeed = WindowsNativeMethodsx64.GetDeviceSpeed;

						 GetMaxPacketSize = WindowsNativeMethodsx64.GetMaxPacketSize;

						 GetMaxIsoPacketSize = WindowsNativeMethodsx64.GetMaxIsoPacketSize;

						 Open = WindowsNativeMethodsx64.Open;

						 Close = WindowsNativeMethodsx64.Close;

						 GetDevice = WindowsNativeMethodsx64.GetDevice;

						 SetConfiguration = WindowsNativeMethodsx64.SetConfiguration;

						 ClaimInterface = WindowsNativeMethodsx64.ClaimInterface;

						 ReleaseInterface = WindowsNativeMethodsx64.ReleaseInterface;

						 OpenDeviceWithVidPid = WindowsNativeMethodsx64.OpenDeviceWithVidPid;

						 SetInterfaceAltSetting = WindowsNativeMethodsx64.SetInterfaceAltSetting;

						 ClearHalt = WindowsNativeMethodsx64.ClearHalt;

						 ResetDevice = WindowsNativeMethodsx64.ResetDevice;

						 AllocStreams = WindowsNativeMethodsx64.AllocStreams;

						 FreeStreams = WindowsNativeMethodsx64.FreeStreams;

						 DevMemAlloc = WindowsNativeMethodsx64.DevMemAlloc;

						 DevMemFree = WindowsNativeMethodsx64.DevMemFree;

						 KernelDriverActive = WindowsNativeMethodsx64.KernelDriverActive;

						 DetachKernelDriver = WindowsNativeMethodsx64.DetachKernelDriver;

						 AttachKernelDriver = WindowsNativeMethodsx64.AttachKernelDriver;

						 SetAutoDetachKernelDriver = WindowsNativeMethodsx64.SetAutoDetachKernelDriver;

						 AllocTransfer = WindowsNativeMethodsx64.AllocTransfer;

						 SubmitTransfer = WindowsNativeMethodsx64.SubmitTransfer;

						 CancelTransfer = WindowsNativeMethodsx64.CancelTransfer;

						 FreeTransfer = WindowsNativeMethodsx64.FreeTransfer;

						 TransferSetStreamId = WindowsNativeMethodsx64.TransferSetStreamId;

						 TransferGetStreamId = WindowsNativeMethodsx64.TransferGetStreamId;

						 ControlTransfer = WindowsNativeMethodsx64.ControlTransfer;

						 BulkTransfer = WindowsNativeMethodsx64.BulkTransfer;

						 InterruptTransfer = WindowsNativeMethodsx64.InterruptTransfer;

						 GetStringDescriptorAscii = WindowsNativeMethodsx64.GetStringDescriptorAscii;

						 TryLockEvents = WindowsNativeMethodsx64.TryLockEvents;

						 LockEvents = WindowsNativeMethodsx64.LockEvents;

						 UnlockEvents = WindowsNativeMethodsx64.UnlockEvents;

						 EventHandlingOk = WindowsNativeMethodsx64.EventHandlingOk;

						 EventHandlerActive = WindowsNativeMethodsx64.EventHandlerActive;

						 InterruptEventHandler = WindowsNativeMethodsx64.InterruptEventHandler;

						 LockEventWaiters = WindowsNativeMethodsx64.LockEventWaiters;

						 UnlockEventWaiters = WindowsNativeMethodsx64.UnlockEventWaiters;

						 WaitForEvent = WindowsNativeMethodsx64.WaitForEvent;

						 HandleEventsTimeout = WindowsNativeMethodsx64.HandleEventsTimeout;

						 HandleEventsTimeoutCompleted = WindowsNativeMethodsx64.HandleEventsTimeoutCompleted;

						 HandleEvents = WindowsNativeMethodsx64.HandleEvents;

						 HandleEventsCompleted = WindowsNativeMethodsx64.HandleEventsCompleted;

						 HandleEventsLocked = WindowsNativeMethodsx64.HandleEventsLocked;

						 PollfdsHandleTimeouts = WindowsNativeMethodsx64.PollfdsHandleTimeouts;

						 GetNextTimeout = WindowsNativeMethodsx64.GetNextTimeout;

						 GetPollfds = WindowsNativeMethodsx64.GetPollfds;

						 FreePollfds = WindowsNativeMethodsx64.FreePollfds;

						 SetPollfdNotifiers = WindowsNativeMethodsx64.SetPollfdNotifiers;

						 HotplugRegisterCallback = WindowsNativeMethodsx64.HotplugRegisterCallback;

						 HotplugDeregisterCallback = WindowsNativeMethodsx64.HotplugDeregisterCallback;
					 }

					 break;
	        }
        }
	}
}
