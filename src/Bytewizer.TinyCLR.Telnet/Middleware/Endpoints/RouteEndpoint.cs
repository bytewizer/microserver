using System;
using System.Collections;

namespace Bytewizer.TinyCLR.Telnet
{
    /// <summary>
    /// Represents an <see cref="Endpoint"/> that can be used in URL matching or URL generation.
    /// </summary>
    public sealed class RouteEndpoint : Endpoint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RouteEndpoint"/> class.
        /// </summary>
        /// <param name="commandDelegate">The delegate used to process requests for the endpoint.</param>
        /// <param name="routePattern">The <see cref="RoutePattern"/> to use in URL matching.</param>
        /// <param name="metadata">The metadata associated with the endpoint.</param>
        /// <param name="displayName">The informational display name of the endpoint.</param>
        public RouteEndpoint(CommandDelegate commandDelegate, string routePattern, Hashtable metadata, string displayName)
            : base(commandDelegate, metadata, displayName)
        {
            if (commandDelegate == null)
            {
                throw new ArgumentNullException(nameof(commandDelegate));
            }

            if (routePattern == null)
            {
                throw new ArgumentNullException(nameof(routePattern));
            }

            RoutePattern = routePattern;
        }

        /// <summary>
        /// Gets the <see cref="RoutePattern"/> associated with the endpoint.
        /// </summary>
        public string RoutePattern { get; }
    }
}