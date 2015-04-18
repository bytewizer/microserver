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
* Basic Authentication (coming soon)
* Extendable Modules
* Form/File Handling
* Static File Handling with File Listing
* Body Decoding (Multipart and UrlEncoded)
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
 ```

## DHCP Service

```csharp
using System;
using System.Net;

using MicroServer.Logging;
using MicroServer.Net.Dhcp;

namespace DhcpServer
{
    public class Program
    {
        // This service creates a full featured Dhcp server
        public static void Main()
        {
            // Initialize logging
            Logger.Initialize(new DebugLogger(), LoggerLevel.Debug);

            //  Create the Dhcp server
            DhcpService DhcpServer = new DhcpService();

            // set the device server name and Dhcp suffix offered to client 
            DhcpServer.ServerName = "example";
            DhcpServer.DnsSuffix = "iot.local";

            // Set a Dhcp pool for the clients to use
            DhcpServer.PoolRange("172.16.10.100", "172.16.10.250");
            DhcpServer.GatewayAddress = "172.16.10.1";
            DhcpServer.SubnetMask = "255.255.255.0";

            // Set a Dhcp reservation that assigns as specific ip address to a client MAC address
            DhcpServer.PoolReservation("172.16.10.15", "000C29027338");

            // Add a NTP server option for time-a.nist.gov as client time source 
            DhcpServer.AddOption(DhcpOption.NTPServer, IPAddress.Parse("129.6.15.28").GetAddressBytes());

            // Sets interface ip address
            DhcpServer.InterfaceAddress = IPAddress.GetDefaultLocalAddress();

            // Starts Dhcp service
            DhcpServer.Start();
        }
    }
}
 ```
 
## DNS Service
 
```csharp
using System;
using System.Net;

using MicroServer.Logging;
using MicroServer.Net.Dns;

namespace DnsServer
{
    public class Program
    {
        // This service creates a Dns server
        public static void Main()
        {
            // Initialize logging
            Logger.Initialize(new DebugLogger(), LoggerLevel.Debug);

            // Create the Dns server
            DnsService DnsServer = new DnsService();

            // Set the device server name and Dns suffix offered to client 
            DnsServer.ServerName = "example";
            DnsServer.DnsSuffix = "iot.local";

            // Sets interface ip address
            DnsServer.InterfaceAddress = IPAddress.GetDefaultLocalAddress();

            // Add a resource record to the zone table 
            Answer record = new Answer();
            record.Domain = string.Concat(DnsServer.ServerName, ".", DnsServer.DnsSuffix);
            record.Class = RecordClass.IN;
            record.Type = RecordType.A;
            record.Ttl = 60;
            record.Record = new ARecord(DnsServer.InterfaceAddress.ToString());
            DnsServer.ZoneFile.Add(record);

            // Enable Dns server to relay requests that can not be looked up locally to another Dns server.
            DnsServer.PrimaryServer = "8.8.8.8";
            DnsServer.IsProxy = true;

            // Starts Dns service
            DnsServer.Start();
        }
    }
}
 ```
 
## SNTP Service
 
 ```csharp
using System;
using System.Net;

using MicroServer.Logging;
using MicroServer.Net.Sntp;

namespace SntpServer
{
    public class Program
    {
        // This service creates a Sntp server that offers time from a relayed server or the local hardware
        public static void Main()
        {
            // Initialize logging
            Logger.Initialize(new DebugLogger(), LoggerLevel.Debug);

            //  Create the Sntp server
            SntpService DnsServer = new SntpService();

            // Enable Sntp server to use local device for it's time reference.
            //DnsServer.UseLocalTimeSource = true;

            // enable Sntp server to relay requests to another Sntp server.
            DnsServer.UseLocalTimeSource = false;
            DnsServer.PrimaryServer = "0.pool.ntp.org";

            // Sets interface ip address
            DnsServer.InterfaceAddress = IPAddress.GetDefaultLocalAddress();

            // Starts Sntp service
            DnsServer.Start();
        }
    }
}
 ```

Contributions
-------------
Any contribution are welcomed

License
-------
Licensed under Apache License 2.0

In the case where this code is a modification of existing code under a separate license, the separate license
terms remain in effect. The modifications to the code are licensed under the Apache license.