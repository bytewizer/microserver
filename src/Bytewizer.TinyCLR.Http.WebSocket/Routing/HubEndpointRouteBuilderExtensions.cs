using System;
using System.Collections;
using System.Diagnostics;
using Bytewizer.TinyCLR.Http.Routing;
using Bytewizer.TinyCLR.Http.WebSockets.Middleware;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Contains extension methods for using Hubs with <see cref="IEndpointRouteBuilder"/>.
    /// </summary>
    public static class HubEndpointRouteBuilderExtensions
    {
        /// <summary>
        /// Adds endpoints for hub actions to the <see cref="IEndpointRouteBuilder"/>.
        /// </summary>
        /// <param name="endpoints">The <see cref="IEndpointRouteBuilder"/>.</param>
        public static IEndpointRouteBuilder MapHub(this IEndpointRouteBuilder endpoints, string pattern, Type hub)
        {
            if (endpoints == null)
            {
                throw new ArgumentNullException(nameof(endpoints));
            }

            return endpoints;
        }

        /// <summary>
        /// Adds endpoints for hub actions to the <see cref="IEndpointRouteBuilder"/> without specifying any routes.
        /// </summary>
        /// <param name="endpoints">The <see cref="IEndpointRouteBuilder"/>.</param>
        public static IEndpointRouteBuilder MapHubs(this IEndpointRouteBuilder endpoints)
        {
            if (endpoints == null)
            {
                throw new ArgumentNullException(nameof(endpoints));
            }

            var hubProvider = new HubEndpointProvider();

            Debug.WriteLine("Map hub route list: ");
            foreach (DictionaryEntry item in hubProvider.GetEndpoints())
            {
                Debug.WriteLine("  url: " + item.Key.ToString());
                endpoints.DataSources.Add(item.Key, item.Value);
            }

            return endpoints;
        }
    }
}
