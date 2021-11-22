# PageBuilder

Enables the ability to build HTML strings including head and body components.

```CSharp
static void Main()
{
    var resources = Resources.ResourceManager;

    _server = new HttpServer(options =>
    {
        options.Pipeline(app =>
        {
            app.UseRouting();
            app.UseResources(resources);
            app.UseEndpoints(endpoints =>
            {          
                endpoints.Map("/favicon.ico", context =>
                {
                    context.Response.SendResource((short)Resources.BinaryResources.Favicon, "image/x-icon", "favicon.ico");
                });

                endpoints.Map("/", context =>
                {
                    var page = new HtmlPage();
                    page.Head.Title = "Hello, world!";
                    page.Head.AdditionalElements.Add("<meta http-equiv='refresh' content='5'>");
                    page.Head.Style = "body { background-color: #68829E } h1 { font-size:2cm; text-align: center; color: #505160;}";
                    page.Body.Content += page.Body.H1Text(DateTime.Now.Ticks.ToString());

                    context.Response.Write(page);
                });
            });
        });
    });
}
```

## TinyCLR Packages
Install release package from [NuGet](https://www.nuget.org/packages?q=bytewizer.tinyclr) or using the Package Manager Console :
```powershell
PM> Install-Package Bytewizer.TinyCLR.Http.PageBuilder
```