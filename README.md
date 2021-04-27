# Microserver for TinyCLR OS

[![Release](https://github.com/microcompiler/microserver/actions/workflows/release.yml/badge.svg)](https://github.com/microcompiler/microserver/actions/workflows/release.yml)
[![Build](https://github.com/microcompiler/microserver/actions/workflows/actions.yml/badge.svg)](https://github.com/microcompiler/microserver/actions/workflows/actions.yml)

Microserver is a modular server built for TinyCLR OS IoT devices inspired by AspNetCore.  Be sure to follow this project with videos on [YouTube](https://youtu.be/EDGo3NpBOpk).

## Socket Services

Build on a modular tcp/udp service with an extendable pipeline and SSL transport security.

* <a href="https://github.com/microcompiler/microserver/tree/develop/src/Bytewizer.TinyCLR.Sockets">Socket Services</a>
* <a href="https://github.com/microcompiler/microserver/tree/develop/src/Bytewizer.TinyCLR.Pipeline">Extendable Pipeline</a> 

## Web Services

Create a modular web services including middleware, moduler routing, digest authentication, controllers, action results, view engine, storage / resource file serving, default file routing, JSON integration, and websockets.

* <a href="https://github.com/microcompiler/microserver/tree/develop/src/Bytewizer.TinyCLR.Http">Http Server</a>
* <a href="https://github.com/microcompiler/microserver/tree/develop/src/Bytewizer.TinyCLR.Http.WebSocket">WebSockets</a>
* <a href="https://github.com/microcompiler/microserver/tree/develop/src/Bytewizer.TinyCLR.Http.Diagnostics">Diagnostics</a> 
* <a href="https://github.com/microcompiler/microserver/tree/develop/src/Bytewizer.TinyCLR.Http.Authentication">Authentication</a>
* <a href="https://github.com/microcompiler/microserver/tree/develop/src/Bytewizer.TinyCLR.Http.Cookies">Cookie Support</a>
* <a href="https://github.com/microcompiler/microserver/tree/develop/src/Bytewizer.TinyCLR.Http.StaticFiles">Static File Handling</a> 
* <a href="https://github.com/microcompiler/microserver/tree/develop/src/Bytewizer.TinyCLR.Http.Mvc">Model-View-Controllers (MVC)</a> 
* <a href="https://github.com/microcompiler/microserver/tree/develop/src/Bytewizer.TinyCLR.Http.Json">JavaScript Object Notation (JSON)</a>
* <a href="https://github.com/microcompiler/microserver/tree/develop/src/Bytewizer.TinyCLR.Http.Cors">Cross-origin Resource Sharing (Cors)</a>

## Runtime libraries
This repo contains several runtime libraries that provide implementations for many general and app-specific types, algorithms, and utility functionality. <a href="https://github.com/microcompiler/runtime">More Information</a>

* <a href="https://github.com/microcompiler/runtime/tree/develop/src/Bytewizer.TinyCLR.Core">Core</a> 
* <a href="https://github.com/microcompiler/runtime/tree/develop/src/Bytewizer.TinyCLR.Logging">Logging</a> 
* <a href="https://github.com/microcompiler/runtime/tree/develop/src/Bytewizer.TinyCLR.IO.Compression">Compression</a> 
* <a href="https://github.com/microcompiler/runtime/tree/develop/src/Bytewizer.TinyCLR.Cryptography">Cryptography</a> 
* <a href="https://github.com/microcompiler/runtime/tree/develop/src/Bytewizer.TinyCLR.DependencyInjection">Dependency Injection (DI)</a> 

## Requirements

**Software:**  <a href="https://visualstudio.microsoft.com/downloads/">Visual Studio 2019</a> and <a href="https://www.ghielectronics.com/">GHI Electronics TinyCLR OS 2.1</a> or higher.  
**Hardware:** Project tested using FEZ Portal single board computers and SCD-20260D development board.  
**External RAM:** Devices with external RAM have the option of extending managed heap into **unsecure** external memory. TinyCLR Config can be used to extend the heap into external SDRAM increasing performance for simultaneous sessions. Please note this feature provides a large amount of managed heap space but data is stored outside of the microcontroller chip where it's less secure.

## Nuget Packages
Prebuild packages are available as attached artifacts on successful [workflow builds](https://github.com/microcompiler/microserver/actions).

## Give a Star! :star:

If you like or are using this project to start your solution, please give it a star. Thanks!

## Getting Started

As we encourage users to play with the samples and test programs this project has not yet reached a release state. See the working [Samples](https://github.com/microcompiler/microserver/tree/master/samples) for an example of how to use packages. The [Playground](https://github.com/microcompiler/microserver/tree/master/playground) also includes many working examples.

### Simple Example

```CSharp
using Bytewizer.TinyCLR.Http;

static void Main()
{
    //Initialize networking

    var server = new HttpServer(options =>
    {
        options.Pipeline(app =>
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.Map("/", context =>
                {
                    string response = "<doctype !html><html><head><title>Hello, world!" +
                        "</title><meta http-equiv='refresh' content='5'></head><body>" +
                        "<h1>" + DateTime.Now.Ticks.ToString() + "</h1></body></html>";

                    context.Response.Write(response);
                });
            });
        });
    });
    server.Start();
}
```

### Controller Example

```CSharp
// Url: http://{ip:port}/example/GetById?id=10

static void Main()
{
    //Initialize networking and sd storage

    var server = new HttpServer(options =>
    {
        options.Pipeline(app =>
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); 
            });
        });
    });
    server.Start();
}

// Page url is automatically created from the controller name and action method.  
public class ExampleController : Controller
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        // called before action method
        if (filterContext.HttpContext.Request.Method != HttpMethods.Get)
        {
            filterContext.Result = new BadRequestResult();
        }
    }

    public override void OnActionExecuted(ActionExecutedContext filterContext)
    {
        // called after action method
    }

    public override void OnException(ExceptionContext filterContext)
    {
        // called on action method execption
        var actionName = ControllerContext.ActionDescriptor.DisplayName;
        
        filterContext.ExceptionHandled = true;
        filterContext.Result = new ContentResult($"An error occurred in the {actionName} action.")
        {
            ContentType = "text/plain",
            StatusCode = StatusCodes.Status500InternalServerError
        };
    }

    // Any public IActionResult method inherited from Controller is made available as an endpoint
    public IActionResult GetById(long id)
    {
        string response = "<doctype !html><html><head><title>Hello, world!</title>" +
            "<style>body { background-color: #111 }" +
            "h1 { font-size:3cm; text-align: center; color: white;}</style></head>" +
            "<body><h1>" + $"{id}" + "</h1></body></html>";

        return Content(response, "text/html");
    }
}
```

**main** :: This is the branch containing the latest release build. No contributions should be made directly to this branch. The development branch will periodically be merged to the master branch, and be released to [NuGet Gallery](https://www.nuget.org).

**develop** :: This is the development branch to which contributions should be proposed by contributors as pull requests. Development build packages are available as attached artifacts on successful [Build](https://github.com/microcompiler/microserver/actions/workflows/actions.yml) workflows.

## Microsoft .NET Micro Framework (NETMF)

Have a look <a href="https://github.com/microcompiler/microserver/releases/tag/v1.1.0"> here </a> if your looking for the original MicroServer built for Microsoft .NET Micro Framework (NETMF).

## Contributions

Contributions to this project are always welcome. Please consider forking this project on GitHub and sending a pull request to get your improvements added to the original project.

## Disclaimer

All source, documentation, instructions and products of this project are provided as-is without warranty. No liability is accepted for any damages, data loss or costs incurred by its use.