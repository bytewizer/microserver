# Static Embedded Resource File Handling
Enables static files inteface for serving files to a client. 

## Samples

The UseResourceFiles() middleware is used to access files embedded within assemblies.

```CSharp
class Program
{    
    static void Main(string[] args)
    { 
        var server = new HttpServer(options =>
        {
            options.Pipeline(app =>
                {
                app.UseResourceFiles(new ResourceFileOptions()
                    {
                        // Resource manager must be included as an option along with a list of resoruces
                        ResourceManager = Resources.ResourceManager,
                        Resources = new Hashtable()
                        {
                            { "/resource.html", Resources.StringResources.Resource }
                        }
                    });
                });
            });
        });
        server.Start();
    }
}
```

## TinyCLR Packages
Install release package from [NuGet](https://www.nuget.org/packages?q=bytewizer.tinyclr) or using the Package Manager Console :
```powershell
PM> Install-Package Bytewizer.TinyCLR.StaticFiles.Resources
```