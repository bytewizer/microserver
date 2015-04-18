using System;
using Microsoft.SPOT;

using MicroServer.Net.Http.Routing;

namespace Test.Harness
{
    public class RouteConfig : HttpApplication
    {
        public override void RegisterRoutes(RouteCollection routes)
        {
            // block access to the 'Deny' folder.
            routes.IgnoreRoute(@"^.*deny.*$");  
            
            // maps the root folder specifically to 'index.html'.
            routes.MapRoute(
                name: "Default",
                regex: @"^[/]{1}$",
                defaults: new DefaultRoute { controller = "Home", action = "Index", id = "" }
            );

            // allows access to all other folders and files not explicitly ignored.
            routes.MapRoute(
                name: "Allow All",
                regex: @".*",
                defaults: new DefaultRoute { controller = "", action = "", id = "" }
            );
        }
    }
}
