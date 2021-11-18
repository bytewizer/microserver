using System;
using System.Collections;
using System.Text;
using System.Threading;

using Bytewizer.TinyCLR.Logging;

namespace Bytewizer.TinyCLR.Hosting
{
    internal static class HostingLoggerExtensions
    {
        public static void ApplicationError(this ILogger logger, EventId eventId, string message, Exception exception)
        {
            logger.Log(
                logLevel: LogLevel.Critical,
                eventId: eventId,
                state: message,
                exception: exception);
        }

        public static void Starting(this ILogger logger)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.Log(
                   logLevel: LogLevel.Debug,
                   eventId: LoggerEventIds.Starting,
                   state: "Hosting starting",
                   exception: null);
            }
        }

        public static void Started(this ILogger logger)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.Log(
                    logLevel: LogLevel.Debug,
                    eventId: LoggerEventIds.Started,
                    state: "Hosting started",
                    exception: null);
            }
        }

        public static void Stopping(this ILogger logger)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.Log(
                    logLevel: LogLevel.Debug,
                    eventId: LoggerEventIds.Stopping,
                    state: "Hosting stopping",
                    exception: null);
            }
        }

        public static void Stopped(this ILogger logger)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.Log(
                    logLevel: LogLevel.Debug,
                    eventId: LoggerEventIds.Stopped,
                    state: "Hosting stopped",
                    exception: null);
            }
        }

        public static void StoppedWithException(this ILogger logger, Exception ex)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.Log(
                    logLevel: LogLevel.Debug,
                    eventId: LoggerEventIds.StoppedWithException,
                    state: "Hosting shutdown exception",
                    exception: ex);
            }
        }

        public static void BackgroundServiceFaulted(this ILogger logger, Exception ex)
        {
            if (logger.IsEnabled(LogLevel.Error))
            {
                logger.Log(
                    logLevel: LogLevel.Error,
                    eventId: LoggerEventIds.BackgroundServiceFaulted,
                    state: "BackgroundService failed",
                    exception: ex);
            }
        }
    }
}
