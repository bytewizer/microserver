Example Micro Framework Console Application
-------------------------------------------
Start a new emulator console application, install Microsoft .NET Micro Framework HTTP Service and create a Program.cs file
with the following source code:

Nuget
-----
```
PM> Install-Package MicroServer.Net.Http
```

## Http File Service
 ```csharp
using System;
using System.Net;

using MicroServer.Logging;
using MicroServer.Net.Http;
using MicroServer.Net.Http.Files;
using MicroServer.Net.Http.Modules;

namespace HttpServer
{
    public class Program
    {       
        // The disk file service allows you to serve and browse files from flash memory/disk storage
        public static void Main()
        {
            // Initialize logging
            Logger.Initialize(new DebugLogger(), LoggerLevel.Debug);

            // Create Http server pipeline  
            ModuleManager ModuleManager = new ModuleManager();

            // Create file/disk service for storage
            DiskFileService fileService = new DiskFileService(@"/", @"\WINFS\");

            // Add the file module to pipeline and enable the file listing feature
            ModuleManager.Add(new FileModule(fileService) { AllowListing = true });

            // Add the error module as the last module to pipeline
            ModuleManager.Add(new ErrorModule());

            //  Create the http server
            HttpService HttpServer = new HttpService(ModuleManager);

            // Sets interface ip address
            HttpServer.InterfaceAddress = IPAddress.GetDefaultLocalAddress();

            // Starts Http service
            HttpServer.Start();
        }
    }
}
 ```

## MVC Web Service
 
 ```csharp
using System;
using System.Net;

using MicroServer.Logging;
using MicroServer.Net.Http;
using MicroServer.Net.Http.Routing;
using MicroServer.Net.Http.Files;
using MicroServer.Net.Http.Modules;
using MicroServer.Net.Http.Mvc;
using MicroServer.Net.Http.Mvc.Controllers;

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

    public class HomeController : Controller
    {
		// Any public IActionResult method inherited from Controller is made available as an endpoint
		public IActionResult Index(ControllerContext context)
        {
            string ipAddress = ServiceManager.Current.HttpService.InterfaceAddress.ToString();
            string port = ServiceManager.Current.HttpService.ActivePort.ToString();
            ViewData["IPADDRESS"] = string.Concat(ipAddress, ":", port);

            return View();
        }    

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
```
Create the following index.html page in '\DOTNETMF_FS_EMULATION\WINFS\WebRoot\Views\Home' directory:

```html
<!DOCTYPE html>
<html>
<head>
    <title>MicroServer Website</title>
    <meta name="author" content="" />
    <meta name="description" content="" />
</head>
<body>
    <div>
        <div>
            <h2>Welcome</h2>
            <p>
                 Here is an example controller endpoint:          
            </p>
            <div>
                <ul>
                    <li><a href="http://[%IPADDRESS%]/home/gethello" target="_blank">GetHello</a></li>
                </ul>
            </div>
        </div>
    </div>
</body>
</html>
```