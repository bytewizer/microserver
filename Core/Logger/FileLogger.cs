
using System;
using System.IO;

using MicroServer.Utilities;

namespace MicroServer.Logging
{
	/// <summary>
	/// FileLogger class
	/// </summary>
	/// <remarks>Implements a file-based data logger for use in the Logger static class.</remarks>
	public class FileLogger : ILogger
	{
		public FileLogger()
		{
			this.isDisposed = false;
			this.sync = new object();
			this.streamWriter = null;
		}

		~FileLogger()
		{
			this.Dispose(false);
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected void Dispose(bool disposing)
		{
			if (this.isDisposed)
				return;

			if (disposing)
			{
				if (this.streamWriter != null)
				{
					this.streamWriter.Flush();
					this.streamWriter.Close();
					this.streamWriter.Dispose();
				}
			}

			this.streamWriter = null;
		}

		public void Open(string directory, string fileName, string fileExt="log")
		{
			lock (this.sync)
			{
				if (this.streamWriter != null)
					this.streamWriter.Close();

				string fullPath = null;
				string name = fileName;
				for (int i = 1; i < 100; ++i)
				{
					fileName = string.Concat(name, i.ToString("D2"), ".", fileExt);
					fullPath = Path.Combine(directory, fileName);
					if (File.Exists(fullPath))
					{
						if (i == 99)
						{
							fileName = string.Concat(name, "01", ".", fileExt);
							fullPath = Path.Combine(directory, fileName);
							break;
						}
					}
					else
					{
						break;
					}
				}

				if (!StringUtility.IsNullOrEmpty(fullPath))
				{
					Directory.CreateDirectory(directory);
					this.streamWriter = new StreamWriter(File.Create(fullPath));
				}

				if (this.streamWriter != null)
				{
					this.streamWriter.WriteLine(string.Concat("Log file opened: ", DateTime.Now.ToString("MM/dd/yyyy | HH:mm:ss.fff")));
					this.streamWriter.Flush();
				}
			}
		}

		public void Close()
		{
			lock (this.sync)
			{
				if (this.streamWriter != null)
				{
					this.streamWriter.Flush();
					this.streamWriter.Close();
					this.streamWriter = null;
				}
			}
		}

        public void WriteError(string message)
        {
            this.WriteToLog(string.Concat("  ", message));
        }

		public void WriteError(object source, string message, Exception ex)
		{
            this.WriteToLog(string.Concat("[", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"), "] ", source.ToString(), " | Error | ", message, " | ", ex.StackTrace.ToString()));
		}

        public void WriteInfo(string message)
        {
            this.WriteToLog(string.Concat("  ", message));
        }

		public void WriteInfo(object source, string message)
		{
            this.WriteToLog(string.Concat("[", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"), "] ", source.ToString(), " | Info  | ", message));
		}

        public void WriteDebug(string message)
        {
            this.WriteToLog(string.Concat("  ", message));
        }

		public void WriteDebug(object source, string message)
		{
            this.WriteToLog(string.Concat("[", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"), "] ", source.ToString(), " | Debug | ", message));
		}

		private void WriteToLog(string message)
		{
			lock (this.sync)
			{
				if (this.streamWriter != null)
				{
					this.streamWriter.WriteLine(message);
					this.streamWriter.Flush();
				}
			}
		}

		private bool isDisposed;
		private object sync = null;
		private StreamWriter streamWriter = null;
	}
}
