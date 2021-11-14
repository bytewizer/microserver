# Embedded Resource File Handling

Enables functionality for serving embedded files within assemblies to a client.

### Resource Properties

Embedded resources must be uploaed in the resource designer as *binary file* types. Rename all files to upload with a ".bin" extention.  Under the "Add Resource" => "Add Exiting Files"  menu  select "All Files (*.*)" to add files to your assemblies.     

![Resource Designer](/images/resources.jpg)

### Simple Example
```CSharp

static void Main()
{
    //Initialize networking
    
    var resourceManager = Resources.ResourceManager;

    var server = new HttpServer(options =>
    {
        options.Pipeline(app =>
        {
            app.UseRouting();
            app.UseResource(resourceManager);
            app.UseEndpoints(endpoints =>
            {          
                endpoints.Map("/favicon.ico", context =>
                {
                    context.Response.SendResource(
                            (short)Resources.BinaryResources.Favicon,
                            "image/x-icon",
                            "favicon.ico"
                        );
                });

                endpoints.Map("/image.jpg", context =>
                {
                    context.Response.SendResource(
                            (short)Resources.BinaryResources.Image,
                            "image/jpeg",
                            "image.jpg"
                        );
                });

                endpoints.Map("/", context =>
                {
                    string response = @"
                        <!DOCTYPE html>
                        <html lang=""en"">
                            <head>
                            <meta charset=""utf-8"">
                            <title>Bytewizer Resorces</title>
                            <style>
                                .item {
                                    text-align: center;
                                }
                                </style>
                            </head>
                            <body>
                            <p class=""item"">
                                <img src=""/image.jpg"" />
                            </p>
                            </body>
                        </html>
                        ";

                    context.Response.Write(response);
                });
            });
        });
    });
}
```

## TinyCLR Packages
Install release package from [NuGet](https://www.nuget.org/packages?q=bytewizer.tinyclr) or using the Package Manager Console :
```powershell
PM> Install-Package Bytewizer.TinyCLR.Http.ResourceManager
```