using System.Collections;

namespace Bytewizer.TinyCLR.Telnet
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
        /// <param name="metadata">The endpoint metadata collection."/>. May be null.</param>
        /// <param name="displayName">The informational display name of the endpoint. May be null.</param>
        public Endpoint(CommandDelegate commandDelegate, Hashtable metadata, string displayName)
        {
            // All are allowed to be null
            CommandDelegate = commandDelegate;
            Metadata = metadata;
            DisplayName = displayName;
        }

        /// <summary>
        /// Gets or sets the informational display name of this endpoint.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets the collection of metadata associated with this endpoint.
        /// </summary>
        public Hashtable Metadata { get; }

        /// <summary>
        /// Gets the delegate used to process requests for the endpoint.
        /// </summary>
        public CommandDelegate CommandDelegate { get; }

        /// <summary>
        /// Returns a string representation of the endpoint.
        /// </summary>
        public override string ToString() => DisplayName ?? base.ToString();
    }
}