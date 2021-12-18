using System.Collections;

namespace Bytewizer.TinyCLR.Telnet
{
    /// <summary>
    /// Defines a contract for a route builder in an application. A route builder specifies the routes for an application.
    /// </summary>
    public interface IEndpointRouteBuilder
    {
        /// <summary>
        /// Gets the endpoint data sources configured in the builder.
        /// </summary>
        Hashtable DataSources { get; }
    }
}