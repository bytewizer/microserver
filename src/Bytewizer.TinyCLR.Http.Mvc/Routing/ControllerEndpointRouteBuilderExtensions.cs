using System;
using System.Collections;
using System.Diagnostics;

using Bytewizer.TinyCLR.Http.Mvc.Middleware;
using Bytewizer.TinyCLR.Http.Routing;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Contains extension methods for using Controllers with <see cref="IEndpointRouteBuilder"/>.
    /// </summary>
    public static class ControllerEndpointRouteBuilderExtensions
    {
        /// <summary>
        /// Adds endpoints for controller actions to the <see cref="IEndpointRouteBuilder"/> without specifying any routes.
        /// </summary>
        /// <param name="endpoints">The <see cref="IEndpointRouteBuilder"/>.</param>
        public static IEndpointRouteBuilder MapControllers(this IEndpointRouteBuilder endpoints)
        {
            if (endpoints == null)
            {
                throw new ArgumentNullException(nameof(endpoints));
            }

            var controllerProvider = new ControllerEndpointProvider();

            Debug.WriteLine("Map controller route list: ");
            foreach (DictionaryEntry item in controllerProvider.GetEndpoints())
            {
                Debug.WriteLine("  url: " + item.Key.ToString());
                endpoints.DataSources.Add(item.Key, item.Value);
            }

            return endpoints;
        }

        /// <summary>
        /// Adds endpoints for controller actions to the <see cref="IEndpointRouteBuilder"/> and specifies a route
        /// with the given <paramref name="name"/>, <paramref name="pattern"/> and <paramref name="defaults"/>.
        /// </summary>
        /// <param name="endpoints">The <see cref="IEndpointRouteBuilder"/> to add the route to.</param>
        /// <param name="name">The name of the route.</param>
        /// <param name="pattern">The URL pattern of the route.</param>
        /// <param name="defaults">
        /// An object that contains default values for route parameters. The object's properties represent the
        /// names and values of the default values.
        /// </param>
        public static IEndpointRouteBuilder MapControllerRoute(this IEndpointRouteBuilder endpoints, string name, string pattern, Route defaults = null)
        {
            if (endpoints == null)
            {
                throw new ArgumentNullException(nameof(endpoints));
            }

            if (string.IsNullOrEmpty(pattern))
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            if (defaults == null)
            {
                throw new ArgumentNullException(nameof(defaults));
            }

            var controllerProvider = new ControllerEndpointProvider();

            // Route pattern should never include leading or trailing slashes
            var routePattern = pattern.TrimStart('/').TrimEnd('/').ToLower();
            var controllerRoute = $"{defaults.controller}/{defaults.action}".ToLower();

            if (controllerProvider.TryGetEndpoint(controllerRoute, out RouteEndpoint endpoint))
            {
                endpoint.DisplayName = name;
                endpoints.DataSources.Add(routePattern, endpoint);
            }
            else 
            {
                throw new InvalidOperationException("Invalid route defaults");
            }
            
            return endpoints;
        }
        
        /// <summary>
        /// Adds endpoints for controller actions to the <see cref="IEndpointRouteBuilder"/> and adds the default route
        /// <c>{controller=Home}/{action=Index}</c>.
        /// </summary>
        /// <param name="endpoints">The <see cref="IEndpointRouteBuilder"/>.</param>
        public static IEndpointRouteBuilder MapDefaultControllerRoute(this IEndpointRouteBuilder endpoints)
        {
            if (endpoints == null)
            {
                throw new ArgumentNullException(nameof(endpoints));
            }

            var controllerProvider = new ControllerEndpointProvider();

            if (controllerProvider.TryGetEndpoint("home/index", out RouteEndpoint endpoint))
            {
                endpoint.DisplayName = "default";
                endpoints.DataSources.Add(string.Empty, endpoint);
            }
            else
            {
                throw new InvalidOperationException("This requires a controller named 'home' and action named 'index'");
            }

            return endpoints;
        }
    }
}