MicroServer
===========

MicroServer is a moduler server built for Microsoft .NET Micro Framework (NETMF).

# Current Features

## HTTP Web Service (MVC style)

Based on jgauffin's <a href="https://github.com/jgauffin/Griffin.WebServer"> MVC Webserver framework. 

* Regex Routing Module
* Custom Controller Handling
* Custom ActionResults (JSON and Dynamic Tokens)
* Custom Modules
* Basic Authentication (coming soon)
* Model Binding (coming soon)
* Views (coming soon)
* Form/File Handling
* Static File Handling with file listing
* Body decoding (multipart & UrlEncoded)
* Header Decoding

## DHCP Service

* Custom Pool Range
* Custom Reservations
* Add/Remove Options
* Simple Packet Debugging

## SNTP Service

* Local (Hardware) Time Source
* Remote (relay) NTP Server Time Source
* Client Synchronize
* Simple Packet Debugging

## DNS Service

* Highly Configurable
* Supported Resource (A, CNAME, MX, NB, NS, PTR, SOA, TXT)
* Simple Packet Debugging

Requirements
------------
Microsoft .NET Micro Framework 4.3 or higher.  

Nuget (coming soon)
-------------------

Available through Nuget (http://www.nuget.org/)

```
PM> Install-Package MicroServer.ServiceManager (coming soon)
```

Example Micro Framework Console Application
-------------------------------------------

Create a RouteConfig.cs file with the following source code:

```csharp
using System;
using Microsoft.SPOT;

using MicroServer.Net.Http.Routing;

namespace MicroServer.Example
{
    public class RouteConfig : HttpApplication
    {
        public override void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                name: "Default",
                regex: @"^[/]{1}$",
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
```

Create a ExampleController.cs file with the following source code:

```csharp
using System;

using MicroServer.Net.Http.Mvc;

namespace MicroServer.Example
{
    public class ExampleController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {

            base.OnActionExecuted(filterContext);
        }

        public override void OnException(ExceptionContext filterContext)
        {

            base.OnException(filterContext);
        }

        public ActionResult GetHello(ControllerContext context)
        {
            string response = "<doctype !html><html><head><title>Hello, world!</title>" +
                "<style>body { background-color: #111 }" +
                "h1 { font-size:3cm; text-align: center; color: white;}</style></head>" +
                "<body><h1>" + DateTime.Now.ToString() + "</h1></body></html>\r\n";

            return new ContentResult(response, "text/html");
        }
    }
}
```
Create a program.cs file with the following source code:

```csharp
using System;
using System.Net;
using Microsoft.SPOT;

using MicroServer.Service;

namespace MicroServer.Example
{
    public class Program
    {
        public static void Main()
        {
            using (ServiceManager Server = new ServiceManager(LogType.Output, LogLevel.Debug, @"\winfs"))
            {  
                //INITIALIZING : Server Services
                Server.InterfaceAddress = System.Net.IPAddress.GetDefaultLocalAddress().ToString();
                Server.ServerName = "example";
                Server.DnsSuffix = "iot.local";

                //SERVICE: DHCP
                Server.DhcpService.PoolRange("172.16.10.100", "172.16.10.254");
                Server.DhcpService.GatewayAddress = "172.16.10.1";
                Server.DhcpService.SubnetMask = "255.255.255.0";

                //SERVICES: Start all serivces
                Server.StartAll();
            } 
        }
    }
}
```
Access the controller endpoint by launching a web browser using http://[emulator ipaddress]/example/gethello

Contributions
------------
Any contribution are welcomed.

License
-------
Licensed under Apache License 2.0

In the case where this code is a modification of existing code under a separate license, the separate license
terms remain in effect. The modifications to the code are licensed under the Apache License 2.0.