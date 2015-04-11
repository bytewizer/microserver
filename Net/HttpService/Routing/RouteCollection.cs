using System;
using System.Collections;
using System.Text.RegularExpressions;

using Microsoft.SPOT;
using MicroServer.Logging;

namespace MicroServer.Net.Http.Routing
{
    /// <summary>
    /// Collection of route rules
    /// </summary>
    public class RouteCollection
    {
        private readonly ArrayList _ingoredRoutes = new ArrayList();
        private readonly ArrayList _mappedRoutes = new ArrayList();

        /// <summary>
        /// Initializes a new instance of the <see cref="RouteCollection"/> class.
        /// </summary>
        public RouteCollection()
        {

        }

        /// <summary>
        /// Gets the ingored routes.
        /// </summary>
        public ArrayList IngoredRoutes
        {
            get { return _ingoredRoutes; }
        }


        /// <summary>
        /// Gets the mapped routes.
        /// </summary>
        public ArrayList MappedRoutes
        {
            get { return _mappedRoutes; }
        }

        /// <summary>
        /// Add ingored routes.
        /// </summary>
        public void IgnoreRoute(string regex)
        {
            _ingoredRoutes.Add(regex);
        }

        /// <summary>
        /// Add mapped routes.
        /// </summary>
        public void MapRoute(string name, string regex, DefaultRoute defaults)
        {
            MappedRoute mappedRoute = new MappedRoute();
            mappedRoute.name = name;
            mappedRoute.regex = regex;
            mappedRoute.defaults = defaults;

            _mappedRoutes.Add(mappedRoute);
        }

        /// <summary>
        /// Match the route and apply the context
        /// </summary>
        /// <param name="context"></param>
        /// <returns>The match results for ingored and mapped routes (<c>null</c> if not existing)</returns>
        public MatchResult Match(IHttpContext context)
        {
            Regex regex;
            Match match;

            Logger.WriteDebug(this, "Match result for ingored and mapped routes:");

            // Process ingored routes
            foreach (string pattern in _ingoredRoutes)
            {
                regex = new Regex(pattern);

                match = regex.Match(context.Request.Uri.AbsolutePath);
                if (match.Success)
                {
                    Logger.WriteDebug("  route pattern:'" + pattern +
                        "' regex:'" + pattern +
                        "' => uri:'" + context.Request.Uri.AbsolutePath +
                        "' = true");

                    return null;
                }
                else
                {
                    Logger.WriteDebug(" route pattern:'" + pattern +
                        "' regex:'" + pattern +
                        "' => uri:'" + context.Request.Uri.AbsolutePath +
                        "' = false");
                }
            }

            // Process mapped routes
            MappedRoute route;

            foreach (MappedRoute item in _mappedRoutes)
            {
                route = (MappedRoute)item;
                regex = new Regex(route.regex);

                match = regex.Match(context.Request.Uri.AbsolutePath);
                if (match.Success)
                {
                    Logger.WriteDebug("  route name:'" + route.name +
                            "' regex:'" + route.regex +
                            "' => uri:'" + context.Request.Uri.AbsolutePath +
                            "' = true");

                    return new MatchResult(match, route);
                }
                else
                {
                    Logger.WriteDebug("  route name:'" + route.name +
                            "' regex:'" + route.regex +
                            "' => uri:'" + context.Request.Uri.AbsolutePath +
                            "' = false");
                }
            }
            return null;
        }
    }
}
