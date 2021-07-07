using System;
using Bytewizer.TinyCLR.Logging;

namespace Bytewizer.TinyCLR.Sntp
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    
    public static class DiagnosticsLoggerExtensions
    {
        public static void InvalidNameResolve(this ILogger logger, string hostName)
        {
            logger.Log(
                LogLevel.Debug,
                new EventId(130, "Time Synchronization"),
                $"Faild to resolve server '{hostName}' to domain name address.",
                null
                );
        }

        public static void TimesyncMessage(this ILogger logger, DateTime time, string hostName)
        {
            logger.Log(
                LogLevel.Information,
                new EventId(131, "Time Synchronization"),
                $"System time {time} (UTC) synchoriztion with {hostName} was completed successfuly.",
                null
                );
        }

        public static void TimesyncException(this ILogger logger, Exception exception)
        {
            logger.Log(
                LogLevel.Error,
                new EventId(132, "Time Synchronization"),
                "An unhandled exception has occurred while synchronizing network time.",
                exception
                );
        }
    }
}
