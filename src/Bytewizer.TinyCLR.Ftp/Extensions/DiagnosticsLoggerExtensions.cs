using Bytewizer.TinyCLR.Logging;

namespace Bytewizer.TinyCLR.Ftp
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    public static class DiagnosticsLoggerExtensions
    {
        private static readonly EventId _commandRequest = new EventId(112, "Request Command Message");
        private static readonly EventId _commandResponse = new EventId(113, "Response Command Message");
        private static readonly EventId _loginSucceeded = new EventId(114, "Login Succeeded");
        private static readonly EventId _loginFailed = new EventId(115, "Login Failed");

        public static void CommandRequest(this ILogger logger, FtpContext context)
        {
            logger.Log(
                LogLevel.Trace,
                _commandRequest,
                $"{context.Connection.LocalEndpoint} <= {context.Connection.RemoteEndpoint } Request:  {context.Request.Command}",
                null
                );
        }

        public static void CommandResponse(this ILogger logger, FtpContext context)
        {
            logger.Log(
                LogLevel.Trace,
                _commandResponse,
                 $"{context.Connection.LocalEndpoint} => {context.Connection.RemoteEndpoint} Response: {context.Response}",
                null
                );
        }

        public static void LoginSucceeded(this ILogger logger, FtpContext context)
        {
            logger.Log(
                LogLevel.Information,
                _loginSucceeded,
                 $"{context.Connection.RemoteEndpoint} Login Succeeded",
                null
                );
        }

        public static void LoginFailed(this ILogger logger, FtpContext context, string message)
        {
            logger.Log(
                LogLevel.Information,
                _loginFailed,
                 $"{context.Connection.RemoteEndpoint} Login Failed: {message}",
                null
                );
        }
    }
}