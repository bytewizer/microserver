# Microserver for TinyCLR OS

Microserver is a modular server built for TinyCLR OS IoT devices inspired by AspNetCore.

[![Build Status](https://img.shields.io/github/workflow/status/microcompiler/microserver/Actions%20CI?style=flat-square&logo=github)](https://github.com/microcompiler/microserver/actions)

### Socket Service

* TCP/UDP support
* Extendable Pipeline
* SSL Transport Support

<a href="https://github.com/microcompiler/microserver/tree/master/src/Bytewizer.TinyCLR.Sockets">More Information</a>

### Web Service

* Extendable Middleware Pipeline
* Header / Cookie Decoding
* Forms / Files Decoding
* Developer Execption Pages

<a href="https://github.com/microcompiler/microserver/tree/master/src/Bytewizer.TinyCLR.Http">More Information</a>

### Model-View-Controllers (MVC)

* Controllers
* Model Binding
* Action Results (Content, Json, Files, Redirects)
* Stubble View Engine
* JSON Integration

<a href="https://github.com/microcompiler/microserver/tree/master/src/Bytewizer.TinyCLR.Http.Mvc">More Information</a>

### Static File Handling

* Storage / Resource File Serving
* Default File Routing

<a href="https://github.com/microcompiler/microserver/tree/master/src/Bytewizer.TinyCLR.Http.StaticFiles">More Information</a>

## Requirements

**Software:**  <a href="https://visualstudio.microsoft.com/downloads/">Visual Studio 2019</a> and <a href="https://www.ghielectronics.com/">GHI Electronics TinyCLR OS 2.0</a> or higher.  
**Hardware:** Project tested using SCD-20260D development board.  
**External RAM:** Devices with external RAM have the option of extending managed heap into **unsecure** external memory. TinyCLR Config can be used to extend the heap into external SDRAM increasing performance for simultaneous sessions. Please note this feature provides a large amount of managed heap space but data is stored outside of the microcontroller chip where it's less secure.

## Give a Star! :star:

If you like or are using this project to start your solution, please give it a star. Thanks!

## Nuget Packages
Release candidate packages can be [downloaded](https://github.com/microcompiler/microserver/releases/tag/v2.0.0-rc.1) from releases as attached asset.  Prebuild packages are available as artifacts on successful [action](https://github.com/microcompiler/microserver/actions) workflow builds.

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

## Branches

**master** - This is the branch containing the latest release - no contributions should be made directly to this branch.

**develop** - This is the development branch to which contributions should be proposed by contributors as pull requests. This development branch will periodically be merged to the master branch, and be released to [NuGet Gallery](https://www.nuget.org).

## Microsoft .NET Micro Framework (NETMF)

Have a look <a href="https://github.com/microcompiler/microserver/releases/tag/v1.1.0"> here </a> if your looking for the original MicroServer built for Microsoft .NET Micro Framework (NETMF).

## Contributions

Contributions to this project are always welcome. Please consider forking this project on GitHub and sending a pull request to get your improvements added to the original project.

## Disclaimer

All source, documentation, instructions and products of this project are provided as-is without warranty. No liability is accepted for any damages, data loss or costs incurred by its use.