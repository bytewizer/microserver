using System;
using Bytewizer.TinyCLR.Logging;

namespace Bytewizer.TinyCLR.Http.Internal
{
    internal static class DiagnosticsLoggerExtensions
    {
        public static void UnhandledException(this ILogger logger, Exception exception)
        {
            logger.LogError(new EventId(1, "UnhandledException"), exception, "An unhandled exception has occurred while executing the request.");
        }

        //public static void ResponseStartedErrorHandler(this ILogger logger)
        //{
        //    logger.LogWarning(new EventId(2, "ResponseStarted"), "The response has already started, the error handler will not be executed.");
        //}

        //public static void ErrorHandlerException(this ILogger logger, Exception exception)
        //{
        //    logger.LogError(new EventId(3, "Exception"), exception, "An exception was thrown attempting to execute the error handler.");
        //}

        //public static void ErrorHandlerNotFound(this ILogger logger)
        //{
        //    logger.LogWarning(new EventId(4, "HandlerNotFound"), "No exception handler was found rethrowing original exception.");
        //}
    }
}
