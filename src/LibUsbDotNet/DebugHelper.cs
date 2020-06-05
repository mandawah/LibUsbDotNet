using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;

namespace LibUsbDotNet
{
	public static class DebugHelper
	{
		public static void WriteLine(object s = null,
			[CallerLineNumber] int lineNumber = 0,
			[CallerMemberName] string caller = null,
			[CallerFilePath] string file = null)
		{
			file = Path.GetFileName(file);
			Console.Error.WriteLine($"{NowString} [P:{Process.GetCurrentProcess().Id:D5}|T:{Thread.CurrentThread.ManagedThreadId:D3}] <{file}:{caller}:{lineNumber}> {s ?? string.Empty}");
		}

		private static string NowString => DateTime.Now.ToString("HH:mm:ss.ffffff");
	}
}
