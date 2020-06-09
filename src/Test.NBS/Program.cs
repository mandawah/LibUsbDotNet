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

			var ptsDevice = context.Find(device => device.VendorId == 0x5AC  && device.ProductId == 0x820B);

			if (ptsDevice == null)
			{
				Console.WriteLine("Device not found");
				return;
			}

			if (!ptsDevice.TryOpen())
			{
				Console.WriteLine("Cannot Open");
				return;
			}

			ptsDevice.SetConfiguration(1);
			
			ptsDevice.ClaimInterface(0);
			//ptsDevice.SetAltInterface(0);

			Console.WriteLine($"Claimed 0!!");

			Thread.Sleep(30*1000);

			Console.WriteLine($"Bye!");

			return;

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
