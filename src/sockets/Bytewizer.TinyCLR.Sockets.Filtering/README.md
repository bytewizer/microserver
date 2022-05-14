# Socket Filtering

Provides a simple methods filtering connections over a network.

```CSharp
static void Main(string[] args)
{
    IServer server = new SocketServer(options =>
    {
        options.Pipeline(app =>
        {
            app.UseIpFiltering("192.168.1.0/24");  // blocks all inbound traffic not within this ip address range
            app.Use(new HttpResponse());
        });
        options.Listen(8080); // Listens on port 8080
    });
    server.Start();
}
 
## TinyCLR Packages
Install release package from [NuGet](https://www.nuget.org/packages?q=bytewizer.tinyclr) or using the Package Manager Console:
```powershell
PM> Install-Package Bytewizer.TinyCLR.Sockets.Filtering
```