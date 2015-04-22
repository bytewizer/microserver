MicroServer
===========

MicroServer is a modular server built for Microsoft .NET Micro Framework (NETMF).

## HTTP Web Service (MVC style)

Based on jgauffin's <a href="https://github.com/jgauffin/Griffin.WebServer"> MVC Web Server framework </a>.

* Regex Routing Module
* Model Binding (coming soon)
* Views with Token Replacement
* Controller Handling
* Action Results (JSON, Content, Files)
* Basic Authentication
* Extendable Modules
* Form/File Handling
* Static File Handling with File Listing
* Body Decoding (Multipart and UrlEncoded)
* Header Decoding
* Cookie Encoding/Decoding

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

* Local Zone File Lookup
* Remote Relay Lookup
* Supported Resource Records (A, CNAME, MX, NB, NS, PTR, SOA, TXT)
* Simple Packet Debugging

Requirements
------------
Software: Microsoft .NET Micro Framework 4.3 or higher.  
Hardware: In addition to the emulator console application example projects, this project was developed 
using<a href="https://www.ghielectronics.com/"> GHI Electronics</a> Raptor and Cobra II mainboards.  


Nuget (coming soon)
-------------------

Available through Nuget (http://www.nuget.org/)

```
PM> Install-Package MicroServer.ServiceManager
```

Example Micro Framework Console Application
-------------------------------------------

This project contains the following simple example (see 'example' folder) project 
demonstrating how to implement a basic server with all modules loaded.  Modules can be
enabled or disabled.

Start a new emulator console application, install MicroServer Service Manager package and create a Program.cs file
with the following source code:

# Using Service Manager to Manage Protocols
```csharp
using System;

using MicroServer.Service;
using MicroServer.Net.Http.Mvc;
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

                // SERVICES:  Enable / disable additional services
                Server.DhcpEnabled = false; // disabled by default as this could be disruptive to existing dhcp servers on the network.
                Server.DnsEnabled = true;
                Server.SntpEnabled = true;

                //SERVICE:  Enable / disable directory browsing
                Server.AllowListing = false;

                //SERVICE: DHCP
                Server.DhcpService.PoolRange("172.16.10.100", "172.16.10.254");
                Server.DhcpService.GatewayAddress = "172.16.10.1";
                Server.DhcpService.SubnetMask = "255.255.255.0";

                //SERVICES: Start all services
                Server.StartAll();
            }
        }
    }
}
```
Access the controller endpoint by launching a web browser using:
http://[emulator ipaddress]/example/gethello

# Using Protocol Modules Individually
If your project only needs to support one or two of the protocols, each protocol service can be loaded without using the service manager.

<a href="https://github.com/microcompiler/microserver/tree/master/Net/HttpService">Http Service</a>

<a href="https://github.com/microcompiler/microserver/tree/master/Net/DhcpService">Dhcp Service</a>

<a href="https://github.com/microcompiler/microserver/tree/master/Net/DnsService">Dns Service</a>

<a href="https://github.com/microcompiler/microserver/tree/master/Net/SntpService">Sntp Service</a>


Contributions
-------------
Any contribution are welcome

License
-------
Licensed under Apache License 2.0

In the case where this code is a modification of existing code under a separate license, the separate license
terms remain in effect. The modifications to the code are licensed under the Apache license.