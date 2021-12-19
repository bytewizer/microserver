using System;
using System.Collections;

using Bytewizer.TinyCLR.Telnet.Features;

namespace Bytewizer.TinyCLR.Telnet
{
    /// <summary>
    /// Extension methods for <see cref="TelnetContext"/> related to routing.
    /// </summary>
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Extension method for getting the commands for the current request.
        /// </summary>
        /// <param name="context">The <see cref="TelnetContext"/> context.</param>
        public static Hashtable GetCommands(this TelnetContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var endpointFeature = (EndpointFeature)context.Features.Get(typeof(EndpointFeature));

            return endpointFeature?.Commands;
        }

        /// <summary>
        /// Extension method for getting the endpoints for the current request.
        /// </summary>
        /// <param name="context">The <see cref="TelnetContext"/> context.</param>
        public static Hashtable GetEndpoints(this TelnetContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var endpointFeature = (EndpointFeature)context.Features.Get(typeof(EndpointFeature));

            return endpointFeature?.Endpoints;
        }

        /// <summary>
        /// Extension method for getting the <see cref="RouteEndpoint"/> for the current request.
        /// </summary>
        /// <param name="context">The <see cref="TelnetContext"/> context.</param>
        public static bool TryGetEndpoint(this TelnetContext context, string pattern, out RouteEndpoint endpoint)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var endpointFeature = (EndpointFeature)context.Features.Get(typeof(EndpointFeature));

            endpoint = default;

            if (endpointFeature.Endpoints == null)
            {
                return false;
            }

            if (endpointFeature.Endpoints.Contains(pattern))
            {
                endpoint = (RouteEndpoint)endpointFeature.Endpoints[pattern];
                return true;
            }

            return false;
        }
    }
}