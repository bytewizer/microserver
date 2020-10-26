using System;
using System.Text;
using System.Collections;

namespace Bytewizer.TinyCLR.Logging
{
    internal class Logger : ILogger
    {
        public LoggerInformation[] Loggers { get; set; }
        
        public MessageLogger[] MessageLoggers { get; set; }

        public void Log(LogLevel logLevel, EventId eventId, object state, Exception exception)
        {
            MessageLogger[] loggers = MessageLoggers;
            if (loggers == null)
            {
                return;
            }

            ArrayList exceptions = null;
            for (int i = 0; i < loggers.Length; i++)
            {
                ref readonly MessageLogger loggerInfo = ref loggers[i];
                if (!loggerInfo.IsEnabled(logLevel))
                {
                    continue;
                }

                LoggerLog(logLevel, eventId, loggerInfo.Logger, exception, state, ref exceptions);
            }

            if (exceptions != null && exceptions.Count > 0)
            {
                ThrowLoggingError(exceptions);
            }
        }

        private static void LoggerLog(LogLevel logLevel, EventId eventId, ILogger logger, Exception exception, object state, ref ArrayList exceptions)
        {
            if (logger == null)
            {
                return;
            }

            try
            {
                logger.Log(logLevel, eventId, state, exception);
            }
            catch (Exception ex)
            {
                if (exceptions == null)
                {
                    exceptions = new ArrayList();
                }

                exceptions.Add(ex);
            }
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            MessageLogger[] loggers = MessageLoggers;
            if (loggers == null)
            {
                return false;
            }

            ArrayList exceptions = null;
            int i = 0;
            for (; i < loggers.Length; i++)
            {
                ref readonly MessageLogger loggerInfo = ref loggers[i];
                if (!loggerInfo.IsEnabled(logLevel))
                {
                    continue;
                }

                if (LoggerIsEnabled(logLevel, loggerInfo.Logger, ref exceptions))
                {
                    break;
                }
            }

            if (exceptions != null && exceptions.Count > 0)
            {
                ThrowLoggingError(exceptions);
            }

            return i < loggers.Length;
        }

        private static bool LoggerIsEnabled(LogLevel logLevel, ILogger logger, ref ArrayList exceptions)
        {
            try
            {
                if (logger.IsEnabled(logLevel))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                if (exceptions == null)
                {
                    exceptions = new ArrayList();
                }

                exceptions.Add(ex);
            }

            return false;
        }
        
        private static void ThrowLoggingError(ArrayList exceptions)
        {
            var msg = new StringBuilder();
            foreach (Exception execption in exceptions)
            {
                msg.AppendLine(execption.Message.ToString());
            }
            
            throw new InvalidOperationException($"An error occurred while writing to logger(s). Message(s): {msg}");
        }
    }
}