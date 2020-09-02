# Microserver for TinyCLR OS

Microserver is a modular server built for TinyCLR OS IoT devices.

[![Build Status](https://img.shields.io/github/workflow/status/microcompiler/microserver/Actions%20CI?style=flat-square&logo=github)](https://github.com/microcompiler/microserver/actions)

## Socket Service

* Tcp/Udp support
* Extendable Pipeline
* Ssl Transport Support

<a href="https://github.com/microcompiler/microserver/tree/master/src/sockets">More Information</a>

## HTTP Web Service

* Controller
* Action Results (Content, Json, Files, Redirects)
* Model Binding
* Header Decoding
* Digest/Basic Authentication
* Extendable Pipeline Middleware
* Static File Handling

## Requirements

**Software:**  <a href="https://visualstudio.microsoft.com/downloads/">Visual Studio 2019</a> and <a href="https://www.ghielectronics.com/">GHI Electronics TinyCLR OS 2.0</a> or higher.  
**Hardware:** Project was tested 
using SC20100S development board.  

## Getting Started

**Work in Progress!** As we encourage users to play with the samples and test programs, this project has not yet reached a stable state. See the [Playground Project](https://github.com/microcompiler/microserver/tree/master/playground) for an example of how to use these packages.

### Getting Started

```CSharp
static void Main()
{
    Networking.SetupEthernet();

    var sdCard = StorageController.FromName(SC20100.StorageController.SdCard);
    var drive = FileSystem.Mount(sdCard.Hdc);

    var server = new HttpServer(options =>
    {
        options.UseMiddleware(new HttpSessionMiddleware());
        options.UseDeveloperExceptionPage();
        options.UseStaticFiles();
        options.UseMvc();
    });
    server.Start();
}
```

## Microsoft .NET Micro Framework (NETMF)

Have a look <a href="https://github.com/microcompiler/microserver/releases/tag/v1.1.0"> here </a> if your looking for the orginal MicroServer built for Microsoft .NET Micro Framework (NETMF).

## Branches

**master** - This is the branch containing the latest release - no contributions should be made directly to this branch.

**develop** - This is the development branch to which contributions should be proposed by contributors as pull requests. This development branch will periodically be merged to the master branch, and be released to [NuGet Gallery](https://www.nuget.org).

## Contributions

Contributions to this project are always welcome. Please consider forking this project on GitHub and sending a pull request to get your improvements added to the original project.

## Disclaimer

All source, documentation, instructions and products of this project are provided as-is without warranty. No liability is accepted for any damages, data loss or costs incurred by its use.
