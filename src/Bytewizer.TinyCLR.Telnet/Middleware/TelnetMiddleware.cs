using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.Pipeline;

using Bytewizer.TinyCLR.Telnet.Features;

namespace Bytewizer.TinyCLR.Telnet
{
    internal class TelnetMiddleware : Middleware
    {
        private readonly ILogger _logger;
        private readonly TelnetServerOptions _telnetOptions;

        private readonly CommandEndpointProvider _endpointProvider;

        public TelnetMiddleware(ILogger logger, TelnetServerOptions options)
        {
            _logger = logger;
            _telnetOptions = options;
            _endpointProvider = new CommandEndpointProvider(options.Assemblies);

            _logger.EndpointRoutes(_endpointProvider);
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

            // Set features objects
            var sessionFeature = new SessionFeature();
            context.Features.Set(typeof(SessionFeature), sessionFeature);

            var endpointFeature = new EndpointFeature()
            {
                Endpoints = _endpointProvider.GetEndpoints(),
                Commands = _endpointProvider.GetCommands(),
            };
            context.Features.Set(typeof(EndpointFeature), endpointFeature);

            next(context);

            var buffer = new byte[1024];
            context.Channel.InputStream.Read(buffer, 0, buffer.Length);
            context.OptionCommands = buffer;

            _ = new TelnetSession(_logger, context, _telnetOptions);
        }
    }
}