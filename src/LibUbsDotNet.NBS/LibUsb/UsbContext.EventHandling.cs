using System;
using System.Threading;

namespace LibUsbDotNet.LibUsb
{
	public sealed partial class UsbContext 
	{
		private Thread m_eventHandlingThread;
		private int m_eventHandlingNeeds;

		public void NeedsEventHandling()
		{
			lock (this)
			{
				if(m_eventHandlingNeeds == 0)
				{
					m_eventHandlingThread = new Thread(HandleEvents)
					{
						IsBackground = true,
						Name = "UsbContext.EventHandling"
					};
				}

				++m_eventHandlingNeeds;
			}
		}

		public void DoesntNeedEventHandlingAnymore()
		{
			lock (this)
			{
				if (m_eventHandlingNeeds == 1)
				{
					NativeMethods.InterruptEventHandler(this.context);
					m_eventHandlingThread.Join();
				}

				m_eventHandlingNeeds = 0;
			}
		}
		
		private void HandleEvents()
		{
			var timeout = new UnixNativeTimeval(60, 0);

			while (true)
			{
				var status =NativeMethods.HandleEventsTimeout(this.context, ref timeout);
				if (status == Error.Interrupted)
				{
					return;
				}
				if (status != Error.Success)
				{
					Console.WriteLine($"Error {status} while handling events");
				}
			}
		}
	}
}
