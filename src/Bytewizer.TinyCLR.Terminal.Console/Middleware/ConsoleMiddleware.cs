using System.Collections;

using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Pipeline;
using Bytewizer.TinyCLR.Terminal.Features;

namespace Bytewizer.TinyCLR.Terminal
{
    internal class ConsoleMiddleware : Middleware
    {
        private readonly ILogger _logger;
        private readonly ConsoleServerOptions _terminalOptions;

        private readonly EndpointProvider _endpointProvider;

        public ConsoleMiddleware(ILogger logger, ConsoleServerOptions options)
        {
            _logger = logger;
            _terminalOptions = options;
            _endpointProvider = new EndpointProvider(options.Assemblies);
        }

        protected override void Invoke(ITerminalContext context, RequestDelegate next)
        {
            var ctx = context as ConsoleContext;
            
            // Set features objects
            var sessionFeature = new SessionFeature();
            ctx.Features.Set(typeof(SessionFeature), sessionFeature);

            var endpointFeature = new EndpointFeature();
            ctx.Features.Set(typeof(EndpointFeature), endpointFeature);

            next(ctx);

            if (endpointFeature.AutoMapping == true)
            {
                foreach (DictionaryEntry endpoint in _endpointProvider.GetEndpoints())
                {
                    endpointFeature.Endpoints.Add(endpoint.Key, endpoint.Value);
                }
            };

            endpointFeature.AvailableCommands = _endpointProvider.GetCommands();

            _ = new ConsoleSession(_logger, ctx, _terminalOptions);

        }
    }
}