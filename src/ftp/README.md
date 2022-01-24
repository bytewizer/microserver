# FTP

Privides a File Transfer Protocol (FTP/FTPS) server to provide simple file transfers built for TinyCLR OS.

## Simple Ftp Server Example

```CSharp
static void Main()
{
    // Initialize networking before starting service

    var server = new FtpServer()
    server.Start();
}
```

## TinyCLR Packages
Install release package from [NuGet](https://www.nuget.org/packages?q=bytewizer.tinyclr) or using the Package Manager Console :
```powershell
PM> Install-Package Bytewizer.TinyCLR.Ftp
```

## RFC - Related Request for Comments 
- [RFC 114 - File Transfer Protocol (FTP)](https://tools.ietf.org/html/rfc114)