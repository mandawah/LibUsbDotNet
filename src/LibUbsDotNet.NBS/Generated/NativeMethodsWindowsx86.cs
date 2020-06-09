//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Copyright © 2006-2010 Travis Robinson. All rights reserved.
// Copyright © 2011-2018 LibUsbDotNet contributors. All rights reserved.
// 
// website: http://github.com/libusbdotnet/libusbdotnet
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
using LibUsbDotNet.LibUsb;
using System;
using System.Runtime.InteropServices;


namespace LibUsbDotNet
{
    internal static unsafe class WindowsNativeMethodsx86
    {
        /// <summary>
        /// Use the default struct alignment for this platform.
        /// </summary>
        internal const int Pack = 0;
        private const string LibUsbNativeLibrary = "libusb-1.0-x86.dll";
        internal const CallingConvention LibUsbCallingConvention = CallingConvention.StdCall;

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_init")]
        public static extern Error Init(ref IntPtr ctx);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_exit")]
        public static extern void Exit(IntPtr ctx);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_set_debug")]
        public static extern void SetDebug(Context ctx, int level);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_get_version")]
        public static extern Version* GetVersion();

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_has_capability")]
        public static extern int HasCapability(uint capability);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_error_name")]
        public static extern IntPtr ErrorName(int errcode);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_setlocale")]
        public static extern Error SetLocale(IntPtr locale);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_strerror")]
        public static extern IntPtr StrError(Error errcode);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_get_device_list")]
        public static extern IntPtr GetDeviceList(Context ctx, ref IntPtr list);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_free_device_list")]
        public static extern void FreeDeviceList(IntPtr list, int unrefDevices);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_ref_device")]
        public static extern Device RefDevice(Device dev);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_unref_device")]
        public static extern void UnrefDevice(IntPtr dev);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_get_configuration")]
        public static extern Error GetConfiguration(DeviceHandle dev, ref int config);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_get_device_descriptor")]
        public static extern Error GetDeviceDescriptor(Device dev, DeviceDescriptor* desc);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_get_active_config_descriptor")]
        public static extern Error GetActiveConfigDescriptor(Device dev, ConfigDescriptor** config);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_get_config_descriptor")]
        public static extern Error GetConfigDescriptor(Device dev, byte configIndex, ConfigDescriptor** config);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_get_config_descriptor_by_value")]
        public static extern Error GetConfigDescriptorByValue(Device dev, byte bconfigurationvalue, ConfigDescriptor** config);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_free_config_descriptor")]
        public static extern void FreeConfigDescriptor(ConfigDescriptor* config);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_get_ss_endpoint_companion_descriptor")]
        public static extern Error GetSsEndpointCompanionDescriptor(ref Context ctx, EndpointDescriptor* endpoint, SsEndpointCompanionDescriptor** epComp);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_free_ss_endpoint_companion_descriptor")]
        public static extern void FreeSsEndpointCompanionDescriptor(SsEndpointCompanionDescriptor* epComp);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_get_bos_descriptor")]
        public static extern Error GetBosDescriptor(DeviceHandle devHandle, BosDescriptor** bos);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_free_bos_descriptor")]
        public static extern void FreeBosDescriptor(BosDescriptor* bos);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_get_usb_2_0_extension_descriptor")]
        public static extern Error GetUsb20ExtensionDescriptor(ref Context ctx, BosDevCapabilityDescriptor* devCap, Usb20ExtensionDescriptor** usb20Extension);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_free_usb_2_0_extension_descriptor")]
        public static extern void FreeUsb20ExtensionDescriptor(Usb20ExtensionDescriptor* usb20Extension);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_get_ss_usb_device_capability_descriptor")]
        public static extern Error GetSsUsbDeviceCapabilityDescriptor(ref Context ctx, BosDevCapabilityDescriptor* devCap, SsUsbDeviceCapabilityDescriptor** ssUsbDeviceCap);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_free_ss_usb_device_capability_descriptor")]
        public static extern void FreeSsUsbDeviceCapabilityDescriptor(SsUsbDeviceCapabilityDescriptor* ssUsbDeviceCap);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_get_container_id_descriptor")]
        public static extern Error GetContainerIdDescriptor(ref Context ctx, BosDevCapabilityDescriptor* devCap, ContainerIdDescriptor** containerId);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_free_container_id_descriptor")]
        public static extern void FreeContainerIdDescriptor(ContainerIdDescriptor* containerId);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_get_bus_number")]
        public static extern byte GetBusNumber(Device dev);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_get_port_number")]
        public static extern byte GetPortNumber(Device dev);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_get_port_numbers")]
        public static extern Error GetPortNumbers(Device dev, byte* portNumbers, int portNumbersLen);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_get_port_path")]
        public static extern Error GetPortPath(Context ctx, Device dev, byte* path, byte pathLength);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_get_parent")]
        public static extern Device GetParent(Device dev);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_get_device_address")]
        public static extern byte GetDeviceAddress(Device dev);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_get_device_speed")]
        public static extern int GetDeviceSpeed(Device dev);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_get_max_packet_size")]
        public static extern int GetMaxPacketSize(Device dev, byte endpoint);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_get_max_iso_packet_size")]
        public static extern int GetMaxIsoPacketSize(Device dev, byte endpoint);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_open")]
        public static extern Error Open(Device dev, ref IntPtr devHandle);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_close")]
        public static extern void Close(IntPtr devHandle);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_get_device")]
        public static extern Device GetDevice(DeviceHandle devHandle);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_set_configuration")]
        public static extern Error SetConfiguration(DeviceHandle devHandle, int configuration);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_claim_interface")]
        public static extern Error ClaimInterface(DeviceHandle devHandle, int interfaceNumber);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_release_interface")]
        public static extern Error ReleaseInterface(DeviceHandle devHandle, int interfaceNumber);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_open_device_with_vid_pid")]
        public static extern DeviceHandle OpenDeviceWithVidPid(Context ctx, ushort vendorId, ushort productId);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_set_interface_alt_setting")]
        public static extern Error SetInterfaceAltSetting(DeviceHandle devHandle, int interfaceNumber, int alternateSetting);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_clear_halt")]
        public static extern Error ClearHalt(DeviceHandle devHandle, byte endpoint);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_reset_device")]
        public static extern Error ResetDevice(DeviceHandle devHandle);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_alloc_streams")]
        public static extern Error AllocStreams(DeviceHandle devHandle, uint numStreams, byte* endpoints, int numEndpoints);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_free_streams")]
        public static extern Error FreeStreams(DeviceHandle devHandle, byte* endpoints, int numEndpoints);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_dev_mem_alloc")]
        public static extern byte* DevMemAlloc(DeviceHandle devHandle, UIntPtr length);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_dev_mem_free")]
        public static extern Error DevMemFree(DeviceHandle devHandle, byte* buffer, UIntPtr length);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_kernel_driver_active")]
        public static extern int KernelDriverActive(DeviceHandle devHandle, int interfaceNumber);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_detach_kernel_driver")]
        public static extern Error DetachKernelDriver(DeviceHandle devHandle, int interfaceNumber);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_attach_kernel_driver")]
        public static extern Error AttachKernelDriver(DeviceHandle devHandle, int interfaceNumber);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_set_auto_detach_kernel_driver")]
        public static extern Error SetAutoDetachKernelDriver(DeviceHandle devHandle, int enable);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_alloc_transfer")]
        public static extern Transfer* AllocTransfer(int isoPackets);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_submit_transfer")]
        public static extern Error SubmitTransfer(Transfer* transfer);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_cancel_transfer")]
        public static extern Error CancelTransfer(Transfer* transfer);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_free_transfer")]
        public static extern void FreeTransfer(Transfer* transfer);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_transfer_set_stream_id")]
        public static extern void TransferSetStreamId(Transfer* transfer, uint streamId);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_transfer_get_stream_id")]
        public static extern uint TransferGetStreamId(Transfer* transfer);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_control_transfer")]
        public static extern int ControlTransfer(DeviceHandle devHandle, byte requestType, byte brequest, ushort wvalue, ushort windex, byte* data, ushort wlength, uint timeout);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_bulk_transfer")]
        public static extern Error BulkTransfer(DeviceHandle devHandle, byte endpoint, byte* data, int length, ref int actualLength, uint timeout);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_interrupt_transfer")]
        public static extern Error InterruptTransfer(DeviceHandle devHandle, byte endpoint, byte* data, int length, ref int actualLength, uint timeout);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_get_string_descriptor_ascii")]
        public static extern Error GetStringDescriptorAscii(DeviceHandle devHandle, byte descIndex, byte* data, int length);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_try_lock_events")]
        public static extern int TryLockEvents(Context ctx);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_lock_events")]
        public static extern void LockEvents(Context ctx);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_unlock_events")]
        public static extern void UnlockEvents(Context ctx);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_event_handling_ok")]
        public static extern int EventHandlingOk(Context ctx);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_event_handler_active")]
        public static extern int EventHandlerActive(Context ctx);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_interrupt_event_handler")]
        public static extern void InterruptEventHandler(Context ctx);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_lock_event_waiters")]
        public static extern void LockEventWaiters(Context ctx);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_unlock_event_waiters")]
        public static extern void UnlockEventWaiters(Context ctx);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_wait_for_event")]
        public static extern int WaitForEvent(Context ctx, ref UnixNativeTimeval tv);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_handle_events_timeout")]
        public static extern Error HandleEventsTimeout(Context ctx, ref UnixNativeTimeval tv);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_handle_events_timeout_completed")]
        public static extern Error HandleEventsTimeoutCompleted(Context ctx, ref UnixNativeTimeval tv, ref int completed);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_handle_events")]
        public static extern Error HandleEvents(Context ctx);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_handle_events_completed")]
        public static extern Error HandleEventsCompleted(Context ctx, ref int completed);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_handle_events_locked")]
        public static extern Error HandleEventsLocked(Context ctx, ref UnixNativeTimeval tv);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_pollfds_handle_timeouts")]
        public static extern Error PollfdsHandleTimeouts(Context ctx);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_get_next_timeout")]
        public static extern Error GetNextTimeout(Context ctx, ref UnixNativeTimeval tv);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_get_pollfds")]
        public static extern Pollfd** GetPollfds(Context ctx);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_free_pollfds")]
        public static extern void FreePollfds(Pollfd** pollfds);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_set_pollfd_notifiers")]
        public static extern void SetPollfdNotifiers(Context ctx, IntPtr addedDelegate, IntPtr removedDelegate, IntPtr userData);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_hotplug_register_callback")]
        public static extern Error HotplugRegisterCallback(Context ctx, HotplugEvent events, HotplugFlag flags, int vendorId, int productId, int devClass, IntPtr Delegate, IntPtr userData, ref int callbackHandle);

        [DllImport(LibUsbNativeLibrary, CallingConvention = LibUsbCallingConvention, EntryPoint = "libusb_hotplug_deregister_callback")]
        public static extern void HotplugDeregisterCallback(Context ctx, int callbackHandle);

    }
}
