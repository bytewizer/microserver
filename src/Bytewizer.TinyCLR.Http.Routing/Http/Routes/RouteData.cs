using System;
using System.Collections;

namespace Bytewizer.TinyCLR.Http.Routing
{
    /// <summary>
    /// Describes information determined during routing that specifies the page to be displayed.
    /// </summary>
    public class RouteData
    {
        /// <summary>
        /// Constructs an instance of <see cref="RouteData"/>.
        /// </summary>
        /// <param name="pageType">The type of the page matching the route, which must implement <see cref="Middleware"/>.</param>
        /// <param name="routeValues">The route parameter values extracted from the matched route.</param>
        public RouteData(Type pageType, ArrayList routeValues)
        {
            if (pageType == null)
            {
                throw new ArgumentNullException(nameof(pageType));
            }

            if (!typeof(Middleware).IsAssignableFrom(pageType))
            {
                throw new ArgumentException($"The value must implement {nameof(Middleware)}.", nameof(pageType));
            }

            PageType = pageType;
            RouteValues = routeValues ?? throw new ArgumentNullException(nameof(routeValues));
        }

        /// <summary>
        /// Gets the type of the page matching the route.
        /// </summary>
        public Type PageType { get; }

        /// <summary>
        /// Gets route parameter values extracted from the matched route.
        /// </summary>
        public ArrayList RouteValues { get; }
    }
}
