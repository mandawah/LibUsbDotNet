using System;
using System.Threading;

namespace LibUsbDotNet.LibUsb
{
	public sealed partial class UsbContext 
	{
		private CancellationTokenSource m_eventHandlingCancellationTokenSource;
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

					m_eventHandlingCancellationTokenSource = new CancellationTokenSource();
					m_eventHandlingThread.Start(m_eventHandlingCancellationTokenSource.Token);
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
					m_eventHandlingCancellationTokenSource.Cancel();
					m_eventHandlingCancellationTokenSource.Dispose();
					m_eventHandlingCancellationTokenSource = null;
					m_eventHandlingThread = null;
				}

				m_eventHandlingNeeds = 0;
			}
		}
		
		private unsafe void HandleEvents(object cancellationToken)
		{
			var timeout = new UnixNativeTimeval(60, 0);
			var cancelToken = (CancellationToken)cancellationToken;

			while (!cancelToken.IsCancellationRequested)
			{
				var status = NativeMethods.HandleEventsTimeout(this.context, ref timeout);
				
				if (status != Error.Success)
				{
					Console.WriteLine($"Error {status} while handling events");
				}
			}

			Console.WriteLine("Terminated");
		}
	}
}
