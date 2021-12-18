using System;
using System.Diagnostics;
using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Sockets.Channel;

namespace Bytewizer.TinyCLR.Sockets
{
//#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    public static class DiagnosticsLoggerExtensions
    {
        public static void UnhandledException(this ILogger logger, string name, string message, Exception exception)
        {
            logger.Log(
                LogLevel.Error,
                new EventId(100, name),
                message,
                exception
                );
        }

        public static void UnhandledException(this ILogger logger, Exception exception)
        {
            logger.Log(
                LogLevel.Error,
                new EventId(100, "Unhandled Exception"),
                "An unhandled exception has occurred while executing the request.",
                exception
                );
        }

        public static void ServiceExecption(this ILogger logger, string message, Exception exception)
        {
            logger.Log(
                LogLevel.Information,
                new EventId(110, "Service Exception"),
                message,
                exception
                );
        }

        public static void StartingService(this ILogger logger, int port)
        {
            var message = $"Started socket listener bound to port {port}.";

            if (logger.GetType() == typeof(NullLogger))
            {
                Debug.WriteLine(message);
            }

            logger.Log(
                LogLevel.Information,
                new EventId(111, "Starting Service"),
                message,
                null
                );
        }

        public static void StartingService(this ILogger logger, int port, Exception exception)
        {
            logger.Log(
                LogLevel.Error,
                new EventId(111, "Starting Service"),
                $"Error starting socket listener bound to port {port}.",
                exception
                );
        }

        public static void StoppingService(this ILogger logger, int port)
        {
            var message = $"Stopping socket listener bound to port {port}.";
            
            if (logger.GetType() == typeof(NullLogger))
            {
                Debug.WriteLine(message);
            }

            logger.Log(
                LogLevel.Information,
                new EventId(112, "Starting Service"),
                message,
                null
                );
        }

        public static void StoppingService(this ILogger logger, int port, Exception exception)
        {
            logger.Log(
                LogLevel.Error,
                new EventId(112, "Starting Service"),
                $"Error stopping socket listener bound to port {port}.",
                exception
                );
        }

        public static void RemoteConnected(this ILogger logger, SocketChannel channel)
        {
            logger.Log(
                LogLevel.Debug,
                new EventId(113, "Remote Connect"),
                $"Remote client {channel.Connection.RemoteIpAddress}:{channel.Connection.RemotePort} has connected.",
                null
                );
        }

        public static void RemoteClosed(this ILogger logger, SocketChannel channel)
        {
            logger.Log(
                LogLevel.Debug,
                new EventId(113, "Remote Connect"),
                $"Remote client {channel.Connection.RemoteIpAddress}:{channel.Connection.RemotePort} has closed connection.",
                null
                );
        }

        public static void RemoteDisconnect(this ILogger logger, Exception exception)
        {
            logger.Log(
                LogLevel.Debug,
                new EventId(113, "Remote Disconnect"),
                $"Remote client has disconnected connection.",
                exception
                );
        }

        public static void ChannelExecption(this ILogger logger, Exception exception)
        {
            logger.Log(
                LogLevel.Error,
                new EventId(114, "Channel Execption"),
                $"Unexpcted channel exception occured.",
                exception
                );
        }

        public static void InvalidMessageLimit(this ILogger logger, long length, long min, long max)
        {
            logger.Log(
                LogLevel.Debug,
                new EventId(114, "Invalid Message Limit"),
                $"Invalid message limit length:{length} min:{min} max:{max}.",
                null
                );
        }

        public static void SocketTransport(this ILogger logger, string message)
        {
            logger.Log(
                LogLevel.Trace,
                new EventId(115, "Socket Transport"),
                message,
                null
                );
        }
    }
}
