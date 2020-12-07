# Microserver for TinyCLR OS

Microserver is a modular server built for TinyCLR OS IoT devices.

[![Build Status](https://img.shields.io/github/workflow/status/microcompiler/microserver/Actions%20CI?style=flat-square&logo=github)](https://github.com/microcompiler/microserver/actions)

## Socket Service

* TCP/UDP support
* Extendable Pipeline
* SSL Transport Support

<a href="https://github.com/microcompiler/microserver/tree/master/src/Bytewizer.TinyCLR.Sockets">More Information</a>

## Web Service
* Extendable Middleware
* Header / Cookie Decoding
* Forms / Files Decoding
* Developer Execption Pages

<a href="https://github.com/microcompiler/microserver/tree/master/src/Bytewizer.TinyCLR.Http">More Information</a>

## Model-View-Controllers (MVC)
* Controllers
* Model Binding
* Action Results (Content, Json, Files, Redirects)
* Stubble View Engine
* JSON Integration

<a href="https://github.com/microcompiler/microserver/tree/master/src/Bytewizer.TinyCLR.Http.Mvc">More Information</a>
## Static File Handling
* Storage / Resource File Serving
* Default File Routing

<a href="https://github.com/microcompiler/microserver/tree/master/src/Bytewizer.TinyCLR.Http.StaticFiles">More Information</a>

## Authentication
* Basic Authentication
* Digest Authentication

<a href="https://github.com/microcompiler/microserver/tree/master/src/Bytewizer.TinyCLR.Http.Authentication">More Information</a>

## Requirements

**Software:**  <a href="https://visualstudio.microsoft.com/downloads/">Visual Studio 2019</a> and <a href="https://www.ghielectronics.com/">GHI Electronics TinyCLR OS 2.0</a> or higher.  
**Hardware:** Project was tested 
using SC20100S development board.  

## Getting Started

**Work in Progress!** As we encourage users to play with the samples and test programs, this project has not yet reached a stable state. See the [Playground Project](https://github.com/microcompiler/microserver/tree/master/playground) for an example of how to use these packages.

## Give a Star! :star:
If you like or are using this project to start your solution, please give it a star. Thanks!

### Getting Started

```CSharp
static void Main()
{
    //Attempts auto-detection of the board initializing networking and sd storage.
    Mainboard.Initialize();

    var server = new HttpServer(options =>
    {
        options.UseDeveloperExceptionPage();
        options.UseFileServer();
        options.UseMvc();
    });
    server.Start();
}
```

```CSharp
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
        filterContext.Result = new ContentResult
        {
            Content = $"An error occurred in the {actionName} action.",
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

Have a look <a href="https://github.com/microcompiler/microserver/releases/tag/v1.1.0"> here </a> if your looking for the orginal MicroServer built for Microsoft .NET Micro Framework (NETMF).

## Contributions

Contributions to this project are always welcome. Please consider forking this project on GitHub and sending a pull request to get your improvements added to the original project.

## Disclaimer

All source, documentation, instructions and products of this project are provided as-is without warranty. No liability is accepted for any damages, data loss or costs incurred by its use.
