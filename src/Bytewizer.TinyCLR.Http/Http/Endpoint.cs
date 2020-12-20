using System.Collections;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Represents a logical endpoint in an application.
    /// </summary>
    public class Endpoint
    {
        /// <summary>
        /// Creates a new instance of <see cref="Endpoint"/>.
        /// </summary>
        /// <param name="displayName">
        /// The informational display name of the endpoint. May be null.
        /// </param>
        public Endpoint(ArrayList metadata, RequestDelegate requestDelegate, string displayName)
        {
            // All are allowed to be null
            RequestDelegate = requestDelegate;
            Metadata = metadata ?? new ArrayList();
            DisplayName = displayName;
        }

        /// <summary>
        /// Gets the informational display name of this endpoint.
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// Gets the delegate used to process requests for the endpoint.
        /// </summary>
        public RequestDelegate RequestDelegate { get; }

        /// <summary>
        /// Gets the collection of metadata associated with this endpoint.
        /// </summary>
        public ArrayList Metadata { get; }

        /// <summary>
        /// Returns a string representation of the endpoint.
        /// </summary>
        public override string ToString() => DisplayName ?? base.ToString();
    }
}
