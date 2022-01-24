using System;
using System.Collections;

using Bytewizer.TinyCLR.Terminal.Features;

namespace Bytewizer.TinyCLR.Terminal
{
    /// <summary>
    /// Extension methods for <see cref="TerminalContext"/> related to routing.
    /// </summary>
    public static class TerminalContextExtensions
    {
        /// <summary>
        /// Extension method for getting the endpoints for the current request.
        /// </summary>
        /// <param name="context">The <see cref="TerminalContext"/> context.</param>
        public static Hashtable GetEndpoints(this TerminalContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var endpointFeature = (EndpointFeature)context.Features.Get(typeof(EndpointFeature));

            return endpointFeature?.Endpoints;
        }

        /// <summary>
        /// Extension method for getting the <see cref="Endpoint"/> for the current request.
        /// </summary>
        /// <param name="context">The <see cref="TerminalContext"/> context.</param>
        /// <param name="pattern">The command endpoint pattern for the current request.</param>
        /// <param name="endpoint">The command endpoint for the current request.</param>
        public static bool TryGetEndpoint(this TerminalContext context, string pattern, out Endpoint endpoint)
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
                endpoint = (Endpoint)endpointFeature.Endpoints[pattern];
                return true;
            }

            return false;
        }
    }
}