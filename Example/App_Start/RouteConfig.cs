using System;
using Microsoft.SPOT;

using MicroServer.Net.Http.Routing;

namespace MicroServer.Example
{
    public class RouteConfig : HttpApplication
    {
        public override void RegisterRoutes(RouteCollection routes)
        {
            // block access to the 'Deny' folder.
            routes.IgnoreRoute(@"^.*Deny.*$");  
            
            // maps the root folder specificly to 'index.html'.
            routes.MapRoute(
                name: "Default",
                regex: @"^[/]{1}$",
                defaults: new DefaultRoute { controller = "", action = "index.html", id = "" }
            );

            // allows access to all other folders and files not explicitly ingored.
            routes.MapRoute(
                name: "Allow All",
                regex: @".*",
                defaults: new DefaultRoute { controller = "", action = "", id = "" }
            );
        }
    }
}
