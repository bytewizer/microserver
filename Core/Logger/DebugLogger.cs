
using System;
using Microsoft.SPOT;

namespace MicroServer.Logging
{
	/// <summary>
	/// DebugLogger class
	/// </summary>
	/// <remarks>Implements a Debug (Output) window based data logger for use in the Logger static class.</remarks>
	public class DebugLogger : ILogger
	{
		public void Dispose()
		{
		}

        public void WriteError(string message)
        {
            this.WriteToLog(message);
        }

		public void WriteError(object source, string message, Exception ex)
		{
            this.WriteToLog(string.Concat("[", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"),"] ", source.ToString(), " | Error | ", message, " | ", ex.StackTrace.ToString()));
		}

        public void WriteInfo(string message)
        {
            this.WriteToLog(message);
        }

		public void WriteInfo(object source, string message)
		{
            this.WriteToLog(string.Concat("[", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"), "] ", source.ToString(), " | Info | ", message));
		}

        public void WriteDebug(string message)
        {
            this.WriteToLog(message);
        }

		public void WriteDebug(object source, string message)
		{
            this.WriteToLog(string.Concat("[", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"), "] ", source.ToString(), " | Debug | ", message));
		}

		private void WriteToLog(string message)
		{
			Debug.Print(message);
		}
	}
}
