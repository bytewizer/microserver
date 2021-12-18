using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.Pipeline;

using Parameters = System.Collections.Hashtable;

using Bytewizer.TinyCLR.Telnet.Features;
using System;
using System.Diagnostics;
using System.Collections;

namespace Bytewizer.TinyCLR.Telnet
{
    internal class TelnetMiddleware : Middleware
    {
        private readonly ILogger _logger;
        private readonly TelnetServerOptions _telnetOptions;

        public TelnetMiddleware(ILogger logger, TelnetServerOptions options)
        {
            _logger = logger;
            _telnetOptions = options;
        }

        protected override void Invoke(TelnetContext context, RequestDelegate next)
        {
            // Check message size
            if (context.Channel.InputStream.Length < _telnetOptions.Limits.MinMessageSize
                || context.Channel.InputStream.Length > _telnetOptions.Limits.MaxMessageSize)
            {
                _logger.InvalidMessageLimit(
                    context.Channel.InputStream.Length,
                    _telnetOptions.Limits.MinMessageSize,
                    _telnetOptions.Limits.MaxMessageSize
                    );

                return;
            }

            var feature = new SessionFeature();
            context.Features.Set(typeof(SessionFeature), feature);

            next(context);

            var buffer = new byte[1024];
            context.Channel.InputStream.Read(buffer, 0, buffer.Length);
            context.OptionCommands = buffer;

            _ = new TelnetSession(_logger, context, _telnetOptions);
        }
    }
}