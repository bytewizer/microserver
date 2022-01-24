using Bytewizer.TinyCLR.Pipeline;
using System.Collections;

namespace Bytewizer.TinyCLR.Terminal
{
    /// <summary>
    /// Represents a logical endpoint in an application.
    /// </summary>
    public class Endpoint
    {
        /// <summary>
        /// Creates a new instance of <see cref="Endpoint"/>.
        /// </summary>
        /// <param name="commandDelegate">The delegate used to process requests for the endpoint.</param>
        /// <param name="routePattern">The route patter to use in endpoint matching.</param>
        /// <param name="instance">The instance implementing the service.</param>
        /// <param name="metadata">The endpoint metadata collection."/>. May be null.</param>
        /// <param name="displayName">The informational display name of the endpoint. May be null.</param>
        public Endpoint(
            RequestDelegate commandDelegate,
            string routePattern,
            object instance,
            Hashtable metadata,
            string displayName)
        {
            // All are allowed to be null
            CommandDelegate = commandDelegate;
            RoutePattern = routePattern;
            Instance = instance;
            Metadata = metadata;
            DisplayName = displayName;
        }

        /// <summary>
        /// Gets the delegate used to process requests for the endpoint.
        /// </summary>
        public RequestDelegate CommandDelegate { get; }

        /// <summary>
        /// Gets the <see cref="RoutePattern"/> associated with the endpoint.
        /// </summary>
        public string RoutePattern { get; }

        /// <summary>
        /// Gets the instance implementing the endpoint.
        /// </summary>
        public object Instance { get; }
        
        /// <summary>
        /// Gets the collection of metadata associated with this endpoint.
        /// </summary>
        public Hashtable Metadata { get; }

        /// <summary>
        /// Gets or sets the informational display name of this endpoint.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Returns a string representation of the endpoint.
        /// </summary>
        public override string ToString() => DisplayName ?? base.ToString();
    }
}