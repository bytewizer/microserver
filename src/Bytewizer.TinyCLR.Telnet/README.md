# Telnet

Privides a general bi-directional telnet server built for TinyCLR OS.

## Simple Telnet Server Example
You can add your own commands and enable password control for connecting to the telnet server.

```CSharp
static void Main()
{
    // Initialize networking before starting service

    var server = new TelnetServer(options =>
    {
        options.Pipeline(app =>
        {
            app.UseAuthentication("bsmith", "password");
        });
    });
    server.Start();
}
```

You can add your custom commands to the telnet service.
```CSharp
public class HelloCommand : Command
{

    // The default IActionResult method is used if no action is provided
    public IActionResult Default()
    {            
        return new ResponseResult("Hello from telnet server!");
    }

    // Any public IActionResult method inherited from Command is made available as command action
    public IActionResult Help()
    {
        var sb = new StringBuilder();
        sb.AppendLine("Available Commands:");
        sb.AppendLine();
        sb.AppendLine(" hello");
        sb.AppendLine();

        return new ResponseResult(sb.ToString()) { NewLine = false };
    }
}
```
## Screenshots
![Telnet Server](../../images/telnet-server.jpg)

## TinyCLR Packages
Install release package from [NuGet](https://www.nuget.org/packages?q=bytewizer.tinyclr) or using the Package Manager Console :
```powershell
PM> Install-Package Bytewizer.TinyCLR.Telnet
PM> Install-Package Bytewizer.TinyCLR.Telnet.Authentication
```

## RFC - Related Request for Comments 
- [RFC 854 - Telnet Protocol Specification](https://tools.ietf.org/html/rfc854)