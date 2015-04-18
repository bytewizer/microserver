using System;
using System.Net;

using MicroServer.Logging;
using MicroServer.Net.Http;
using MicroServer.Net.Http.Routing;
using MicroServer.Net.Http.Files;
using MicroServer.Net.Http.Modules;
using MicroServer.Net.Http.Mvc;
using MicroServer.Net.Http.Mvc.Controllers;
using MicroServer.Net.Http.Authentication;

namespace WebServer
{
    public class Program
    {
        // The routing and controller service allows you to serve create flexible custom controllers
        public static void Main()
        {
            // Initialize logging
            Logger.Initialize(new DebugLogger(), LoggerLevel.Debug);

            // Create Http server pipeline  
            ModuleManager ModuleManager = new ModuleManager();

            // Add the router module as the fist module to pipeline
            ModuleManager.Add(new RouterModule());

            // Add the controller module to pipeline
            ModuleManager.Add(new ControllerModule());

            // Add the error module as the last module to pipeline
            ModuleManager.Add(new ErrorModule());

            //  Create the Http server
            HttpService HttpServer = new HttpService(ModuleManager);

            // Sets interface ip address
            HttpServer.InterfaceAddress = IPAddress.GetDefaultLocalAddress();

            // Starts Http service
            HttpServer.Start();
        }
    }

    public class RouteConfig : HttpApplication
    {
        public override void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                name: "Default",
                regex: @"^[/]{1}$",
                defaults: new DefaultRoute { controller = "example", action = "gethello", id = "" }
            );

            routes.MapRoute(
                name: "Allow All",
                regex: @".*",
                defaults: new DefaultRoute { controller = "", action = "", id = "" }
            );
        }
    }

    public class ExampleController : Controller
    {
        // Any public IActionResult method inherited from Controller is made available as an endpoint
        public IActionResult GetHello(ControllerContext context)
        {
            string response = "<doctype !html><html><head><title>Hello, world!</title>" +
                "<style>body { background-color: #111 }" +
                "h1 { font-size:3cm; text-align: center; color: white;}</style></head>" +
                "<body><h1>" + DateTime.Now.ToString() + "</h1></body></html>\r\n";

            return ContentResult(response, "text/html");
        }
    }
}
