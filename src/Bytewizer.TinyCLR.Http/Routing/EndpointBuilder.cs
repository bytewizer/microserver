using System.Collections;

namespace Bytewizer.TinyCLR.Http.Routing
{
    /// <summary>
    /// A base class for building an new <see cref="Endpoint"/>.
    /// </summary>
    public abstract class EndpointBuilder
    {
        /// <summary>
        /// Gets or sets the delegate used to process requests for the endpoint.
        /// </summary>
        public RequestDelegate RequestDelegate { get; set; }

        /// <summary>
        /// Gets or sets the informational display name of this endpoint.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets the collection of metadata associated with this endpoint.
        /// </summary>
        public ArrayList Metadata { get; } = new ArrayList();

        /// <summary>
        /// Creates an instance of <see cref="Endpoint"/> from the <see cref="EndpointBuilder"/>.
        /// </summary>
        /// <returns>The created <see cref="Endpoint"/>.</returns>
        public abstract Endpoint Build();
    }
}
