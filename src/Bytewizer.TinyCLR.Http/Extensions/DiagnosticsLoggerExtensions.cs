using System;
using Bytewizer.TinyCLR.Logging;

namespace Bytewizer.TinyCLR.Http
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    
    public static class DiagnosticsLoggerExtensions
    {
        public static void InvalidRequestMethod(this ILogger logger, string method)
        {
            logger.Log(
                LogLevel.Debug,
                new EventId(210, "Invalid Request Method"),
                $"The request method {method} are not supported.",
                null
                );
        }

        public static void InvalidRequestPath(this ILogger logger, string path)
        {
            logger.Log(
                LogLevel.Debug,
                new EventId(211, "Invalid Request Path"),
                $"The request path {path} does not match the path filter.",
                null
                );
        }

        public static void InvalidFileType(this ILogger logger, string path)
        {
            logger.Log(
                LogLevel.Debug,
                new EventId(212, "Invalid File Type"),
                $"The request path {path} does not match a supported file type.",
                null
                );
        }

        public static void InvalidFilePath(this ILogger logger, string path)
        {
            logger.Log(
                LogLevel.Debug,
                new EventId(213, "Invalid File Path"),
                $"The request path {path} does not match an existing file",
                null
                );
        }
    }
}
