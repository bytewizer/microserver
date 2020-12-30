using System;
using System.Collections;

namespace Bytewizer.TinyCLR.Http.Routing
{
    /// <summary>
    /// Supports building a new <see cref="RouteEndpoint"/>.
    /// </summary>
    public sealed class RouteEndpointBuilder : EndpointBuilder
    {
        /// <summary>
        /// Constructs a new <see cref="RouteEndpointBuilder"/> instance.
        /// </summary>
        /// <param name="requestDelegate">The delegate used to process requests for the endpoint.</param>
        /// <param name="routePattern">The <see cref="RoutePattern"/> to use in URL matching.</param>
        public RouteEndpointBuilder(RequestDelegate requestDelegate, string routePattern)
        {
            RequestDelegate = requestDelegate;
            RoutePattern = routePattern;
        }

        /// <summary>
        /// Gets or sets the <see cref="RoutePattern"/> associated with this endpoint.
        /// </summary>
        public string RoutePattern { get; set; }

        /// <inheritdoc />
        public override Endpoint Build()
        {
            if (RequestDelegate is null)
            {
                throw new InvalidOperationException($"{nameof(RequestDelegate)} must be specified to construct a {nameof(RouteEndpoint)}.");
            }

            var routeEndpoint = new RouteEndpoint(RequestDelegate, RoutePattern, null, DisplayName);

            return routeEndpoint;
        }
    }
}