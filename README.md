# Microserver for TinyCLR OS

[![NuGet Status](http://img.shields.io/nuget/v/Bytewizer.TinyCLR.Core.svg?style=flat&logo=nuget)](https://www.nuget.org/packages?q=bytewizer.tinyclr)
[![Release](https://github.com/bytewizer/microserver/actions/workflows/release.yml/badge.svg)](https://github.com/bytewizer/microserver/actions/workflows/release.yml)
[![Build](https://github.com/bytewizer/microserver/actions/workflows/actions.yml/badge.svg)](https://github.com/bytewizer/microserver/actions/workflows/actions.yml)


Microserver is a modular embedded server built for TinyCLR OS IoT devices.  Be sure to follow this project on our [YouTube](https://www.youtube.com/channel/UCfFRHPY9XEsfIC0pLTSJ8kw) channel. 

## Network Services

Build modular tcp/udp service with an extendable pipeline and SSL transport security.

* <a href="https://github.com/bytewizer/microserver/tree/develop/src/Bytewizer.TinyCLR.TcpClient">TCP Client Services</a>
* <a href="https://github.com/bytewizer/microserver/tree/develop/src/Bytewizer.TinyCLR.Sockets">TCP/UDP Socket Server</a>
* <a href="https://github.com/bytewizer/microserver/tree/develop/src/Bytewizer.TinyCLR.Sntp">Simple Network Time Protocol (SNTP) Server</a>


## Web Services

Create a modular web services including middleware, moduler routing, digest authentication, controllers, action results, view engine, storage / resource file serving, default file routing, JSON integration, and websockets.

* <a href="https://github.com/bytewizer/microserver/tree/develop/src/Bytewizer.TinyCLR.Http">HTTP Server</a>
* <a href="https://github.com/bytewizer/microserver/tree/develop/src/Bytewizer.TinyCLR.Http.Diagnostics">Diagnostics</a> 
* <a href="https://github.com/bytewizer/microserver/tree/develop/src/Bytewizer.TinyCLR.Http.Authentication">Authentication</a>
* <a href="https://github.com/bytewizer/microserver/tree/develop/src/Bytewizer.TinyCLR.Http.Cookies">Cookie Support</a>
* <a href="https://github.com/bytewizer/microserver/tree/develop/src/Bytewizer.TinyCLR.Http.ResourceManager">Resource Manager Handling</a>
* <a href="https://github.com/bytewizer/microserver/tree/develop/src/Bytewizer.TinyCLR.Http.PageBuilder">Html Response Builder</a>
* <a href="https://github.com/bytewizer/microserver/tree/develop/src/Bytewizer.TinyCLR.Http.StaticFiles">Static File Handling</a>
* <a href="https://github.com/bytewizer/microserver/tree/develop/src/Bytewizer.TinyCLR.Http.StaticFiles.Blazor">Blazor File Serving</a>  
* <a href="https://github.com/bytewizer/microserver/tree/develop/src/Bytewizer.TinyCLR.Http.StaticFiles.Resource">Embedded Resource File Serving</a>
* <a href="https://github.com/bytewizer/microserver/tree/develop/src/Bytewizer.TinyCLR.Http.Mvc">Model-View-Controllers (MVC)</a> 
* <a href="https://github.com/bytewizer/microserver/tree/develop/src/Bytewizer.TinyCLR.Http.Json">JavaScript Object Notation (JSON)</a>
* <a href="https://github.com/bytewizer/microserver/tree/develop/src/Bytewizer.TinyCLR.Http.Cors">Cross-origin Resource Sharing (CORS)</a>

## Runtime libraries
This  <a href="https://github.com/bytewizer/runtime">repo</a> contains several runtime libraries built for TinyCLR OS. These libraries provide implementations for logging, middleware and dependency injection.

## Nuget Packages
Install release package from [NuGet](https://www.nuget.org/packages?q=bytewizer.tinyclr). Preview packages are available as attached artifacts on successful build [workflow](https://github.com/bytewizer/microserver/actions).

## Requirements

**Software:**  <a href="https://visualstudio.microsoft.com/downloads/">Visual Studio 2019</a> and <a href="https://www.ghielectronics.com/">GHI Electronics TinyCLR OS 2.1</a> or higher.  
**Hardware:** Project tested using FEZ Portal, FEZ Duino single board computers and SCD-20260D development board.  
**External RAM:** Devices with external RAM have the option of extending managed heap into **unsecure** external memory. TinyCLR Config can be used to extend the heap into external SDRAM increasing performance for simultaneous sessions. Please note this feature provides a large amount of managed heap space but data is stored outside of the microcontroller chip where it's less secure.

## Give a Star! :star:

If you like or are using this project to start your solution, please give it a star. Thanks!

## Getting Started

We encourage you to play with the samples and test programs. See the working [Samples](https://github.com/bytewizer/microserver/tree/master/samples) for an example of how to use packages. The [Playground](https://github.com/bytewizer/microserver/tree/master/playground) also includes many working examples.

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

## Continuous Integration

**master** :: This is the branch containing the latest release build. No contributions should be made directly to this branch. The development branch will periodically be merged to the master branch, and be released to [NuGet](https://www.nuget.org/packages?q=bytewizer.tinyclr).

**develop** :: This is the development branch to which contributions should be proposed by contributors as pull requests. Development build packages are available as attached artifacts on successful build [workflows](https://github.com/bytewizer/microserver/actions/workflows/actions.yml).


## Microsoft .NET Micro Framework (NETMF)

Have a look <a href="https://github.com/bytewizer/microserver/releases/tag/v1.1.0"> here </a> if your looking for the original MicroServer built for Microsoft .NET Micro Framework (NETMF).

## Contributions

Contributions to this project are always welcome. Please consider forking this project on GitHub and sending a pull request to get your improvements added to the original project.

## Disclaimer

All source, documentation, instructions and products of this project are provided as-is without warranty. No liability is accepted for any damages, data loss or costs incurred by its use.