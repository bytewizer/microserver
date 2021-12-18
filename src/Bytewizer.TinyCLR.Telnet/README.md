# Telnet

Privides a Telnet server a fairly general, bi-directional communications facility built for TinyCLR OS.

## Simple Telnet Server Example

```CSharp
static void Main()
{
    // Initialize networking before starting service

    var server = new TelnetServer()
    server.Start();
}
```

## TinyCLR Packages
Install release package from [NuGet](https://www.nuget.org/packages?q=bytewizer.tinyclr) or using the Package Manager Console :
```powershell
PM> Install-Package Bytewizer.TinyCLR.Telnet
```

## RFC - Related Request for Comments 
- [RFC 854 - Telnet Protocol Specification](https://tools.ietf.org/html/rfc854)