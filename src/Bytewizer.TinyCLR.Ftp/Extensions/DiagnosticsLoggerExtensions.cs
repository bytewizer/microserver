﻿using System.Diagnostics;
using Bytewizer.TinyCLR.Logging;

namespace Bytewizer.TinyCLR.Ftp
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    public static class DiagnosticsLoggerExtensions
    {
        private static readonly EventId _commandRequest = new EventId(112, "Request Command Message");
        private static readonly EventId _commandResponse = new EventId(113, "Response Command Message");

        public static void CommandRequest(this ILogger logger, FtpContext context)
        {
            logger.Log(
                LogLevel.Debug,
                _commandRequest,
                $"Command Request:  {context.Request.Command}",
                null
                );
        }

        public static void CommandResponse(this ILogger logger, FtpContext context)
        {
            logger.Log(
                LogLevel.Debug,
                _commandResponse,
                 $"Command Response: {context.Response}",
                null
                );
        }
    }
}