using Bytewizer.TinyCLR.Http.Routing;

namespace Bytewizer.TinyCLR.Http.Features
{
    /// <summary>
    /// A feature interface for routing functionality.
    /// </summary>
    public interface IRoutingFeature
    {
        /// <summary>
        /// Gets or sets the <see cref="Routing.RouteData"/> associated with the current request.
        /// </summary>
        RouteData RouteData { get; set; }
    }
}
