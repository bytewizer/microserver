using System;

namespace MicroServer.Net.Http.Routing
{
    /// <summary>
    /// Implementation base for <see cref="RouteConfig"/>
    /// </summary>
    public abstract class HttpApplication
    {
        public virtual void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                name: "Allow All",
                regex: @".*",
                defaults: new DefaultRoute { controller = "", action = "", id = "" }
            );
        }
    }
}
