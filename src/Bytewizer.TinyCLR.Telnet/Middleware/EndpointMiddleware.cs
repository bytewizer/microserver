using System.Diagnostics;
using System.Collections;

using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Pipeline;


namespace Bytewizer.TinyCLR.Telnet
{
    internal class EndpointMiddleware : Middleware
    {
        private readonly ILogger _logger;
        private readonly CommandEndpointProvider _endpointProvider;

        public EndpointMiddleware()
            :this (NullLogger.Instance)
        {
        }

        public EndpointMiddleware(ILogger logger)
        {
            _logger = logger;
            _endpointProvider = new CommandEndpointProvider();
        }

        protected override void Invoke(TelnetContext context, RequestDelegate next)
        {
            Debug.WriteLine("Map command endpoint list: ");

            foreach (DictionaryEntry item in _endpointProvider.GetEndpoints())
            {
                //_logger.Log()
                Debug.WriteLine("  endpoint: " + item.Key.ToString());

            }

            next(context);
        }
    }
}