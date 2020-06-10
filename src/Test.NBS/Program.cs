using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LibUbsDotNet.NBS.TransferBufferAllocationManagement;
using LibUsbDotNet;
using LibUsbDotNet.Info;
using LibUsbDotNet.LibUsb;

namespace TestUsbNBS
{
	class Program
	{
		static async Task Main(string[] args)
		{
			//UsbContext.Default.SetDebugLevel(LogLevel.Debug);

			var deviceList = UsbContext.Default.GetDeviceList();
			
			foreach (var device in deviceList)
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

			//0xDEB1

			var ptsDevice = deviceList.FirstOrDefault(device => device.VendorId == 0x1500 && device.ProductId == 0xDEB1);

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

			ptsDevice.DetachKernelDriver(0);
			ptsDevice.DetachKernelDriver(1);
			Console.WriteLine($"Detached 0!!");

			if(ptsDevice.GetConfiguration() != 1)
			{
				ptsDevice.SetConfiguration(1);
			}

			//Console.WriteLine($"SetConfiguration 1!!");

			ptsDevice.ClaimInterface(1);

			Console.WriteLine($"Claimed!!");
			
			ptsDevice.SetAltInterface(1, 0);

			var reporter = new ThroughputReporter();

			var bulkReader = new AsyncBulkTransferRunner(ptsDevice, 0x81, ByteArrayCopyReadTransferManagement.CreateManagements(4,
				data=>
				{
					//Console.WriteLine($"Received {data.Length:D2} bytes : {new string(data.Select(b => (char)b).ToArray()),40}");
					reporter.AddReceptionCount(data.Length);
					return default;
				},
				error =>
				{
					Console.WriteLine($"Error {error}");
				}));

			bulkReader.Start();

			await Task.Delay(10*1000);

			Console.WriteLine($"Dispose");

			bulkReader.Dispose();

			Console.WriteLine($"Bye!");
		}

		private static string GetDescriptorReport(UsbDevice usbDevice)
		{
			if (!usbDevice.TryOpen()) return "Not able to open";
			
			var sbReport = new StringBuilder();

			sbReport.AppendLine(string.Format("{0} OSVersion:{1} LibUsbDotNet Version:{2} DriverMode:{3}", usbDevice.Info.SerialNumber, Environment.OSVersion, LibUsbDotNetVersion, null));
			sbReport.AppendLine(usbDevice.Info.ToString());
			foreach (UsbConfigInfo cfgInfo in usbDevice.Configs)
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
			
			usbDevice.Close();

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

	public class ThroughputReporter : IDisposable
    {
		private readonly CancellationTokenSource m_cancellationTokenSource = new CancellationTokenSource();

		private long m_sentTotalCount;
		private long m_sentTotalCall;

		private long m_receptionTotalCount;
		private long m_receptionTotalCall;

		private long m_firstTick = -1;

		public ThroughputReporter()
		{
			Run(m_cancellationTokenSource.Token);
		}

		public void AddReceptionCount(int count)
		{
			Interlocked.Add(ref m_receptionTotalCount, count);
			Interlocked.Increment(ref m_receptionTotalCall);

			if (m_firstTick == -1)
			{
				m_firstTick = DateTime.Now.Ticks;
			}
		}

		public void AddSentCount(int count)
		{
			Interlocked.Add(ref m_sentTotalCount, count);
			Interlocked.Increment(ref m_sentTotalCall);

			if (m_firstTick == -1)
			{
				m_firstTick = DateTime.Now.Ticks;
			}
		}

		private Task Run(CancellationToken token)
		{
			return Task.Factory.StartNew(
				async () =>
				{
					long lastTick = DateTime.Now.Ticks;

					long lastReceptionCount = 0;
					long lastSentCount = 0;

					long lastReceptionCall = 0;
					long lastSentCall = 0;

					while (!token.IsCancellationRequested)
					{
						var totalSentCount = Interlocked.Read(ref m_sentTotalCount);
						var totalReceptionCount = Interlocked.Read(ref m_receptionTotalCount);

						var totalSentCall = Interlocked.Read(ref m_sentTotalCall);
						var totalReceptionCall = Interlocked.Read(ref m_receptionTotalCall);

						var nowTick = DateTime.Now.Ticks;

						var totalElapsedTick = nowTick - m_firstTick;

						var receptionCountDiff = totalReceptionCount - lastReceptionCount;
						var sentCountDiff = totalSentCount - lastSentCount;

						var receptionCallDiff = totalReceptionCall - lastReceptionCall;
						var sentCallDiff = totalSentCall - lastSentCall;

						var elapsed = nowTick - lastTick;

						lastTick = nowTick;
						lastReceptionCount = totalReceptionCount;
						lastSentCount = totalSentCount;
						lastReceptionCall = totalReceptionCall;
						lastSentCall = totalSentCall;

						Console.WriteLine($"Rx: {MbPerSecond(receptionCountDiff, elapsed):N2} MByte/s [{Mb(m_receptionTotalCount):N2} Mbytes]  Tx: {MbPerSecond(sentCountDiff, elapsed):N2} MByte/s [{Mb(m_sentTotalCount):N2} Mbytes]    (Rx #: {KPerSec(receptionCallDiff, elapsed):N2} K   Tx #: {KPerSec(sentCallDiff, elapsed):N2} K)");

						await Task.Delay(1000, token);
					}
				}, TaskCreationOptions.LongRunning | TaskCreationOptions.HideScheduler);
		}

		private double MbPerSecond(long count, long ticks)
		{
			return (1d * count / (1024 * 1024)) * 10000000 / ticks;
		}

		private double Mb(long count)
		{
			return (1d * count / (1024 * 1024));
		}

		private double KPerSec(long count, long ticks)
		{
			return (1d * count / (1000)) * 10000000 / ticks;
		}

		public void Dispose()
		{
			m_cancellationTokenSource.Cancel();
			m_cancellationTokenSource.Dispose();
		}
    }
}
