using System;

namespace MicroServer.Logging
{
	public static class Logger 
	{
        private static object lockObject = new object();
        private static ILogger logger = null;
        private static LoggerLevel maxLogLevel = LoggerLevel.Error;
        
        public static void Initialize(ILogger newLogger, LoggerLevel logLevel)
		{
			if (logger != null)
			{
				throw new InvalidOperationException("Logger may only be initialized once");
			}
			logger = newLogger;
			maxLogLevel = logLevel;
		}

		public static LoggerLevel GetLogLevel()
		{
			return maxLogLevel;
		}

		public static void SetLogLevel(LoggerLevel logLevel)
		{
			lock (lockObject)
			{
				maxLogLevel = logLevel;
			}
		}

        public static void WriteError(string message)
        {
            lock (lockObject)
            {
                if ((int)maxLogLevel >= (int)LoggerLevel.Error)
                    logger.WriteError(message);
            }
        }

		public static void WriteError(object source, string message, Exception ex)
		{
			lock (lockObject)
			{
				if ((int)maxLogLevel >= (int)LoggerLevel.Error)
					logger.WriteError(source, message, ex);
			}
		}

        public static void WriteInfo(string message)
        {
            lock (lockObject)
            {
                if ((int)maxLogLevel >= (int)LoggerLevel.Info)
                    logger.WriteInfo(message);
            }
        }

		public static void WriteInfo(object source, string message)
		{
			lock (lockObject)
			{
				if ((int)maxLogLevel >= (int)LoggerLevel.Info)
					logger.WriteInfo(source, message);
			}
		}
        public static void WriteDebug(string message)
        {
            lock (lockObject)
            {
                if ((int)maxLogLevel >= (int)LoggerLevel.Debug)
                    logger.WriteDebug(message);
            }
        }

		public static void WriteDebug(object source, string message)
		{
			lock (lockObject)
			{
				if ((int)maxLogLevel >= (int)LoggerLevel.Debug)
					logger.WriteDebug(source, message);
			}
		}
	}
}
