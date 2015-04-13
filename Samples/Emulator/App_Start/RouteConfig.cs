using System;
using Microsoft.SPOT;

using MicroServer.Net.Http.Routing;

namespace MicroServer.Emulator
{
    public class RouteConfig : HttpApplication
    {
        public override void RegisterRoutes(RouteCollection routes)
        {
            //routes.IgnoreRoute(@".*");  // Block all routes
            routes.IgnoreRoute(@"\css$");  // Block all routes to the css folder

            routes.MapRoute(
                name: "Default",
                regex: @"\/$",
                defaults: new DefaultRoute { controller = "", action = "index.html", id = "" }
            );

            routes.MapRoute(
                name: "Json",
                regex: @"\json$",
                defaults: new DefaultRoute { controller = "Test", action = "GetJson", id = "" }
            );

            routes.MapRoute(
                name: "Allow All",
                regex: @".*",
                defaults: new DefaultRoute { controller = "", action = "", id = "" }
            );

            //routes.MapRoute(
            //    name: "Deny All",
            //    regex: @"$a",
            //    defaults: new DefaultRoute { controller = "", action = "", id = "" }
            //);
        }
    }
}
