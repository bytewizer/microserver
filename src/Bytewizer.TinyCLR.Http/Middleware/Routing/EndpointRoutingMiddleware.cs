using System;
using System.Collections;

using Bytewizer.TinyCLR.Http.Routing;
using Bytewizer.TinyCLR.Http.Features;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Enables routing feature to match the URLs of incoming requests.
    /// </summary>
    public class EndpointRoutingMiddleware : Middleware
    {
        private readonly Hashtable _endpointDataSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="EndpointRoutingMiddleware"/> class.
        /// </summary>
        public EndpointRoutingMiddleware(IEndpointRouteBuilder endpointRouteBuilder)
        {
            if (endpointRouteBuilder == null)
            {
                throw new ArgumentNullException(nameof(endpointRouteBuilder));
            }

            _endpointDataSource = endpointRouteBuilder.DataSources;
        }

        /// <inheritdoc/>
        protected override void Invoke(HttpContext context, RequestDelegate next)
        {
            // There's already an endpoint skip maching completely
            var endpoint = context.GetEndpoint();
            if (endpoint != null)
            {
                // Log match skipped;
                next(context);
            }

            string path = context.Request.Path.TrimStart('/').TrimEnd('/').ToLower();
            
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

            next(context);
        }
    }
}