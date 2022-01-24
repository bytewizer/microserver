using System.Collections;

using Bytewizer.TinyCLR.Pipeline;
using Bytewizer.TinyCLR.Terminal.Features;

namespace Bytewizer.TinyCLR.Terminal
{
    /// <summary>
    /// Enables endpoint routing features for incoming requests.
    /// </summary>
    public class EndpointMiddleware : Middleware
    {
        private readonly ServerCommand[] _serverCommand;
        private readonly EndpointProvider _endpointProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="EndpointMiddleware"/> class.
        /// </summary>
        public EndpointMiddleware(ServerCommand[] serverCommand)
        {
            _serverCommand = serverCommand;
            _endpointProvider= new EndpointProvider(null);
        }

        /// <inheritdoc/>
        protected override void Invoke(ITerminalContext context, RequestDelegate next)
        {
            var endpointFeature = (EndpointFeature)context.Features.Get(typeof(EndpointFeature));

            foreach (ServerCommand endpoint in _serverCommand)
            {
                _endpointProvider.MapCommandActions(endpoint.GetType());             
            }

            foreach (DictionaryEntry endpoint in _endpointProvider.GetEndpoints())
            {
                endpointFeature.Endpoints.Add(endpoint.Key, endpoint.Value);
            }

            next(context);
        }
    }
}