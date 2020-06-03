using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LibUsbDotNet;
using LibUsbDotNet.Info;
using LibUsbDotNet.LibUsb;
using LibUsbDotNet.Main;

namespace TestEmbeddedUsb
{
	class Program
	{
		static void Main(string[] args)
		{
			var context = new UsbContext();
			context.SetDebugLevel(LogLevel.Debug);

			context.StartHandlingEvents();
			
			foreach (var device in context.List())
			{
				string sAdd = string.Format("Vid:0x{0:X4} Pid:0x{1:X4} (rev:{2})",
					device.Info.VendorId,
					device.Info.ProductId,
					(ushort)device.Info.Usb);

				Console.WriteLine($"We have:  {sAdd}");
				Console.WriteLine($"{GetDescriptorReport(device)}");
				Console.WriteLine();
			}

			//var ptsFinder = new UsbDeviceFinder(0x0A12, 0x1);

			var ptsDevice = context.Find(device => device.VendorId == 0xA12 && device.ProductId == 0x1);

			if (ptsDevice == null)
			{
				Console.WriteLine("PTS not found");
				return;
			}

			if (!ptsDevice.TryOpen())
			{
				Console.WriteLine("Cannot Open");
				return;
			}

			var defaultConfig = ptsDevice.Configs[0];
			
			var largestPacketSize = 0;
			var bestAltSet = 0;
			var bestInterface = 0;
			UsbEndpointInfo bestEndpoint = null;

			foreach (var iface in defaultConfig.Interfaces)
			{
				foreach (var endpointInfo in iface.Endpoints)
				{
					if (endpointInfo.EndpointAddress != 0x83)
					{
						continue;
					}

					if (endpointInfo.MaxPacketSize > largestPacketSize)
					{
						largestPacketSize = endpointInfo.MaxPacketSize;
						bestInterface = iface.Number;
						bestAltSet = iface.AlternateSetting;
						bestEndpoint = endpointInfo;
					}
				}
			}

			Console.WriteLine($"Largest packet size: {largestPacketSize}");

			Console.WriteLine($"Best {bestInterface} / {bestAltSet} / {PrintEndpoint(bestEndpoint)}");

			ptsDevice.SetConfiguration(1);
			
			ptsDevice.ClaimInterface(0);
			ptsDevice.SetAltInterface(0);

			ptsDevice.ClaimInterface(bestInterface);
			ptsDevice.SetAltInterface(bestAltSet);

			//ptsDevice.SetAltInterface(0); 
			//largestPacketSize = 9;

			var aclReader = ptsDevice.OpenEndpointReader((ReadEndpointID)0x82, -1, EndpointType.Bulk);

			var aclBuffer = new byte[64 * 1]; 

			DebugHelper.WriteLine("Before ACL Read");
			aclReader.Read(aclBuffer, 0, aclBuffer.Length, 3000, out var aclRead);
			DebugHelper.WriteLine($"ACL Read {aclRead}");

			var scoReader = ptsDevice.OpenEndpointReader((ReadEndpointID)bestEndpoint.EndpointAddress, -1, (EndpointType)(bestEndpoint.Attributes & 0x3));

			var scoBuffer = new byte[largestPacketSize * 1];

			for (var i = 0; i < 1; ++i)
			{
				DebugHelper.WriteLine($"Before SCO Read");
				scoReader.Read(scoBuffer, 0, scoBuffer.Length, 0, out var scoRead);
				DebugHelper.WriteLine($"SCO Read {scoRead}");
			}
			
			ptsDevice.ReleaseInterface(bestInterface);
			ptsDevice.ReleaseInterface(0);

			ptsDevice.Close();
			ptsDevice.Dispose();

			DebugHelper.WriteLine();
			//context.StopHandlingEvents();
			DebugHelper.WriteLine();
		}

		private static string GetDescriptorReport(IUsbDevice usbRegistry)
		{
			if (!usbRegistry.TryOpen()) return "Not able to open";
			
			var sbReport = new StringBuilder();

			sbReport.AppendLine(string.Format("{0} OSVersion:{1} LibUsbDotNet Version:{2} DriverMode:{3}", usbRegistry.Info.SerialNumber, Environment.OSVersion, LibUsbDotNetVersion, null));
			sbReport.AppendLine(usbRegistry.Info.ToString());
			foreach (UsbConfigInfo cfgInfo in usbRegistry.Configs)
			{
				sbReport.AppendLine(string.Format("CONFIG #{1}\r\n{0}", cfgInfo.ToString(), cfgInfo.ConfigurationValue));
				foreach (UsbInterfaceInfo interfaceInfo in cfgInfo.Interfaces)
				{
					sbReport.AppendLine($"INTERFACE {PrintInterface(interfaceInfo)}");

					foreach (UsbEndpointInfo endpointInfo in interfaceInfo.Endpoints)
					{
						sbReport.AppendLine($"ENDPOINT: {PrintEndpoint(endpointInfo)}");
					}

					sbReport.AppendLine();
				}

				sbReport.AppendLine();
			}
			
			usbRegistry.Close();

			return sbReport.ToString();
		}

		private static string PrintEndpoint(UsbEndpointInfo info)
		{
			return $"Addr: 0x{info.EndpointAddress:X2}; Attr: 0x{info.Attributes:X2}; Inter: {info.Interval}; MaxPkt: {info.MaxPacketSize}; Rfrsh: {info.Refresh}; SyncAdd: {info.SyncAddress}";
		}

		private static string PrintInterface(UsbInterfaceInfo info)
		{
			return $"Intfc: {info.Interface}; Num: {info.Number}; Alt: {info.AlternateSetting}; Class: {info.Class}; SubClass: {info.SubClass}; Prot: {info.Protocol}";
		}

		public static string LibUsbDotNetVersion
		{
			get
			{
				Assembly assembly = Assembly.GetAssembly(typeof(UsbDevice));
				string[] assemblyKvp = assembly.FullName.Split(',');
				foreach (string s in assemblyKvp)
				{
					string[] sKeyPair = s.Split('=');
					if (sKeyPair[0].ToLower().Trim() == "version")
					{
						return sKeyPair[1].Trim();
					}
				}
				return null;
			}
		}
	}
}
