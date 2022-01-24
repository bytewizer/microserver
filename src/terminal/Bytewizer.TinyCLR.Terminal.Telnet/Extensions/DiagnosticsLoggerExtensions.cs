using System.Text;
using System.Collections;

using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Terminal.Features;

namespace Bytewizer.TinyCLR.Terminal
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    public static class DiagnosticsLoggerExtensions
    {
        private static readonly EventId _commandRequest = new EventId(112, "Request Command Message");
        private static readonly EventId _commandResponse = new EventId(113, "Response Command Message");
        private static readonly EventId _loginSucceeded = new EventId(114, "Login Succeeded");
        private static readonly EventId _loginFailed = new EventId(115, "Login Failed");

        public static void CommandRequest(this ILogger logger, TelnetContext context, byte[] buffer, int count)
        {
            var request = buffer.ToEncodedString(count);
            
            logger.Log(
                LogLevel.Trace,
                _commandRequest,
                $"{context.Connection.LocalEndpoint} <= {context.Connection.RemoteEndpoint } Request: {request}",
                null
                );
        }

        public static void CommandExecuted(this ILogger logger, TelnetContext context, Endpoint endpoint)
        {
            logger.Log(
                LogLevel.Trace,
                _commandRequest,
                $"{context.Connection.LocalEndpoint} => {context.Connection.RemoteEndpoint } Executing Endpoint: {endpoint.RoutePattern}",
                null
                );
        }

        public static void CommandResponse(this ILogger logger, TelnetContext context)
        {
            logger.Log(
                LogLevel.Trace,
                _commandResponse,
                 $"{context.Connection.LocalEndpoint} => {context.Connection.RemoteEndpoint} Response: {context.Response}",
                null
                );
        }

        public static void LoginSucceeded(this ILogger logger, TelnetContext context)
        {
            var sessionFeature = (SessionFeature)context.Features.Get(typeof(SessionFeature));

            logger.Log(
                LogLevel.Information,
                _loginSucceeded,
                 $"Login succeeded for {sessionFeature.UserName} from remote clinet {context.Connection.RemoteEndpoint}.",
                null
                );
        }

        public static void LoginFailed(this ILogger logger, TelnetContext context, string message)
        {
            logger.Log(
                LogLevel.Information,
                _loginFailed,
                 $"Login failed from remote clinet {context.Connection.RemoteEndpoint}. {message}",
                null
                );
        }

        //public static void EndpointRoutes(this ILogger logger, EndpointProvider provider)
        //{
        //    var sb = new StringBuilder();
        //    sb.Append($"Reflection Endpoints:");

        //    if (provider.GetEndpoints().Count >= 1)
        //    {
        //        foreach (DictionaryEntry item in provider.GetEndpoints())
        //        {
        //            sb.Append($" {item.Key}");
        //        }

        //        logger.Log(
        //            LogLevel.Information,
        //            _loginSucceeded,
        //             sb.ToString(),
        //            null
        //            );

        //        //sb.Clear();
        //        //sb.Append($"Commands:");
        //        //foreach (DictionaryEntry item in provider.GetCommands())
        //        //{
        //        //    var description = string.IsNullOrEmpty(item.Value.ToString()) ? "none" : item.Value.ToString();          
        //        //    sb.Append($" {item.Key}[{description}]");
        //        //}

        //        //logger.Log(
        //        //    LogLevel.Information,
        //        //    _loginSucceeded,
        //        //     sb.ToString(),
        //        //    null
        //        //    );
        //    }
        //}
    }
}