# Cors

Enables cross-origin resource sharing (CORS) control capabilities with OPTIONS preflight.

## Simple Cors Example
```CSharp
var server = new HttpServer(options =>
{
    options.Pipeline(app =>
    {
        //defaults to Access-Control-Allow-Origin = "*"
        app.UseCors(); 
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.Map("/", context => 
            {
                context.Response.Write("Cores Example"); 
            });
        });
    });
});
server.Start();
```

## Cors Example with Origin, Header and Method Validation
```CSharp
var server = new HttpServer(options =>
{
    options.Pipeline(app =>
    {
        app.UseCors(new CorsOptions()
        {
            Origins = "http://device.example.local:8080",
            Methods = "get,put",
            Headers = "content-type,accept"
        });
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.Map("/", context => 
            {
                context.Response.Write("Cores Example"); 
            });
        });
    });
});
server.Start();
```

## TinyCLR Packages
Install release package from [NuGet](https://www.nuget.org/packages?q=bytewizer.tinyclr) or using the Package Manager Console :
```powershell
PM> Install-Package Bytewizer.TinyCLR.Http.Cors
```

## RFC - Related Request for Comments 
- [RFC 6454 - The Web Origin Concept](https://tools.ietf.org/html/rfc6454)