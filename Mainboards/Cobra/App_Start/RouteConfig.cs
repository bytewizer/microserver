using System;
using Microsoft.SPOT;

using MicroServer.Net.Http.Routing;

namespace MicroServer.CobraII
{
    public class RouteConfig : HttpApplication
    {
        public override void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                name: "Default",
                regex: @"\/$",
                defaults: new DefaultRoute { controller = "", action = "index.html", id = "" }
            );

            routes.MapRoute(
                name: "Allow All",
                regex: @".*",
                defaults: new DefaultRoute { controller = "", action = "", id = "" }
            );

        }
    }
}
