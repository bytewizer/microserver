using System;
using System.Collections;

namespace Bytewizer.TinyCLR.Http.Routing
{
    /// <summary>
    /// Provides extension methods for <see cref="IEndpointRouteBuilder"/> to add endpoints.
    /// </summary>
    public static class EndpointRouteBuilderExtensions
    {
        public static void MapControllerRoute(this IEndpointRouteBuilder endpoints, string name, string url, Route defaults)
        {
            if (endpoints == null)
            {
                throw new ArgumentNullException(nameof(endpoints));
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            MappedRoute mappedRoute = new MappedRoute
            {
                Name = name,
                Pattern = url,
                Defaults = defaults
            };

            ((ArrayList)endpoints.DataSources).Add(mappedRoute);

        }

        public static void MapDefaultControllerRoute(this IEndpointRouteBuilder endpoints)
        {
            if (endpoints == null)
            {
                throw new ArgumentNullException(nameof(endpoints));
            }
        }
    }
}