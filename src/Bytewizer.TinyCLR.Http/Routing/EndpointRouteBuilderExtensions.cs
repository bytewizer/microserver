using System;

using Bytewizer.TinyCLR.Http.Routing;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Provides extension methods for <see cref="IEndpointRouteBuilder"/> to add endpoints.
    /// </summary>
    public static class EndpointRouteBuilderExtensions
    {
        /// <summary>
        /// Adds a <see cref="RouteEndpoint"/> to the <see cref="IEndpointRouteBuilder"/> that matches HTTP requests
        /// for the specified pattern.
        /// </summary>
        /// <param name="endpoints">The <see cref="IEndpointRouteBuilder"/> to add the route to.</param>
        /// <param name="pattern">The route pattern.</param>
        /// <param name="requestDelegate">The delegate executed when the endpoint is matched.</param>
        public static IEndpointRouteBuilder Map(this IEndpointRouteBuilder endpoints, string pattern, RequestDelegate requestDelegate)
        {
            if (endpoints == null)
            {
                throw new ArgumentNullException(nameof(endpoints));
            }

            if (string.IsNullOrEmpty(pattern))
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            if (requestDelegate == null)
            {
                throw new ArgumentNullException(nameof(requestDelegate));
            }

            // Route pattern should never include leading or trailing slashes
            var routePattern = pattern.TrimStart('/').TrimEnd('/').ToLower();

            var builder = new RouteEndpointBuilder(requestDelegate, routePattern)
            {
                DisplayName = pattern,
            };

            // Catch error and log when same endpoint is used.  Endpoints must have unique pattern. key exists
            endpoints.DataSources.Add(routePattern, builder.Build());

            return endpoints;
        }
    }
}
