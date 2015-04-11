
using System;

namespace MicroServer.Logging
{
	public interface ILogger : IDisposable
	{
        void WriteInfo(string message);
        void WriteInfo(object source, string message);
        void WriteDebug(string message);
		void WriteDebug(object source, string message);
        void WriteError(string message);
		void WriteError(object source, string message, Exception ex);
	}
}
