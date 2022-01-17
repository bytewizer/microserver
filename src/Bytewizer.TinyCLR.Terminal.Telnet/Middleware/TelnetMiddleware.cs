using System.Collections;

using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.Pipeline;
using Bytewizer.TinyCLR.Terminal.Features;

namespace Bytewizer.TinyCLR.Terminal
{
    internal class TelnetMiddleware : Middleware
    {
        private readonly ILogger _logger;
        private readonly TelnetServerOptions _terminalOptions;

        private readonly EndpointProvider _endpointProvider;

        public TelnetMiddleware(ILogger logger, TelnetServerOptions options)
        {
            _logger = logger;
            _terminalOptions = options;
            _endpointProvider = new EndpointProvider(options.Assemblies);
        }

        protected override void Invoke(ITerminalContext context, RequestDelegate next)
        {
            var ctx = context as TelnetContext;
            // Check message size
            if (ctx.Channel.InputStream.Length < _terminalOptions.Limits.MinMessageSize
                || ctx.Channel.InputStream.Length > _terminalOptions.Limits.MaxMessageSize)
            {
                _logger.InvalidMessageLimit(
                    ctx.Channel.InputStream.Length,
                    _terminalOptions.Limits.MinMessageSize,
                    _terminalOptions.Limits.MaxMessageSize
                    );

                return;
            }

            // Set features objects
            var sessionFeature = new SessionFeature();
            context.Features.Set(typeof(SessionFeature), sessionFeature);

            var endpointFeature = new EndpointFeature();
            context.Features.Set(typeof(EndpointFeature), endpointFeature);

            next(context);

            if (endpointFeature.AutoMapping == true)
            {
                foreach(DictionaryEntry endpoint in _endpointProvider.GetEndpoints())
                {
                    endpointFeature.Endpoints.Add(endpoint.Key, endpoint.Value);
                }
            };

            endpointFeature.AvailableCommands = _endpointProvider.GetCommands();

            _ = new TelnetSession(_logger, ctx, _terminalOptions);
        }
    }
}