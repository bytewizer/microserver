using System;
using System.Collections;

using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Http.Routing;
using Bytewizer.TinyCLR.Http.Features;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Enables routing feature to match the URLs of incoming requests.
    /// </summary>
    public class EndpointRoutingMiddleware : Middleware
    {
        //private readonly ILogger _logger;
        private readonly Hashtable _endpointDataSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="EndpointRoutingMiddleware"/> class.
        /// </summary>
        /// <param name="endpointRouteBuilder">The endpoint data sources configured in the builder.</param>
        public EndpointRoutingMiddleware( IEndpointRouteBuilder endpointRouteBuilder)
            : this (NullLoggerFactory.Instance, endpointRouteBuilder)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EndpointRoutingMiddleware"/> class.
        /// </summary>
        /// <param name="loggerFactory">The factory used to create loggers.</param>
        /// <param name="endpointRouteBuilder">The endpoint data sources configured in the builder.</param>
        public EndpointRoutingMiddleware(ILoggerFactory loggerFactory, IEndpointRouteBuilder endpointRouteBuilder)
        {
            if (endpointRouteBuilder == null)
            {
                throw new ArgumentNullException(nameof(endpointRouteBuilder));
            }

            //_logger = loggerFactory.CreateLogger("Bytewizer.TinyCLR.Http");
            _endpointDataSource = endpointRouteBuilder.DataSources;
        }

        /// <inheritdoc/>
        protected override void Invoke(HttpContext context, RequestDelegate next)
        {
            // There's already an endpoint skip maching completely
            var endpoint = context.GetEndpoint();
            if (endpoint != null)
            {
                //_logger.LogDebug($"Endpoint '{endpoint.DisplayName}' already set, skipping route matching.");
                next(context);
            }

            string path = context.Request.Path.TrimStart('/').TrimEnd('/').ToLower();
            
            // TODO: Build matcher object
            var routeEndpoint = (Endpoint)_endpointDataSource[path];
            if (routeEndpoint != null)
            {
                var endpointFeature = new EndpointFeature
                {
                    Endpoint = new Endpoint(
                        routeEndpoint.RequestDelegate,
                        routeEndpoint.Metadata,
                        routeEndpoint.DisplayName)
                };

                context.Features.Set(typeof(IEndpointFeature), endpointFeature);

                // Match found continue pipeline        
                next(context);
            }
            else
            {
                //_logger.LogDebug($"Request did not match any endpoints");
            }

            next(context);
        }
    }
}