using Bytewizer.TinyCLR.Http.Routing;

namespace Bytewizer.TinyCLR.Http.Features
{
    /// <summary>
    /// A feature for routing functionality.
    /// </summary>
    public class RoutingFeature : IRoutingFeature
    {
        /// <inheritdoc />
        public RouteData RouteData { get; set; }
    }
}
