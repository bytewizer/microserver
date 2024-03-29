# Static File Handling
Enables static files inteface for serving files to a client. 

## Static File Server
The UseFileServer() combines the functionality of UseStaticFiles() and UseDefaultFiles() middleware.

```CSharp
class Program
{
    static void Main(string[] args)
    {
        // Storage devices must be mounted to access sd cards
        var sd = StorageController.FromName(SC20100.StorageController.SdCard);
        FileSystem.Mount(sd.Hdc);
        
        var server = new HttpServer(options =>
        {
            options.Pipeline(app =>
                {
                    app.UseFileServer();
                });
            });
        server.Start();
    }
}
```

The default provider includes only html, x-icon, gif, jpeg, png, javascript, json, and font-woff content type mappings. To create a custom mapping between file extensions and MIME types.

```CSharp
options.Pipeline(app =>
{
    app.UseFileServer(new DefaultContentTypeProvider(
        new Hashtable()
        {
            { ".gif", "image/gif" },
            ...
            { ".woff2", "font/woff2" },
    }));
});
```

Here is a <a href="CONTENTTYPES.md"> full list</a> of MIME types available to easly add to your project.


## Static Files

Its the responsibility of UseStaticFiles() middleware is to look for a file path (e.g. images/image.jpeg) and serve content from this folder. The UseDefaultFiles() is a URL rewriter that doesn't actually serve the file. UseDefaultFiles() must be called before UseStaticFiles() to serve the default file **Index.html** or **Index.htm**

```CSharp
class Program
{
    static void Main(string[] args)
    {
        // Storage devices must be mounted to access sd cards
        var sd = StorageController.FromName(SC20100.StorageController.SdCard);
        FileSystem.Mount(sd.Hdc);
        
        var server = new HttpServer(options =>
        {
            options.Pipeline(app =>
                {
                    // UseDefaultFiles() must be called before UseStaticFile().
                    app.UseDefaultFiles();
                    app.UseStaticFiles();
                });
            });
        });
        server.Start();
    }
}
```

To set default.html as a default page displayed on root access. Use DefaultFilesOptions method as shown below.
```CSharp
app.UseDefaultFiles(new DefaultFilesOptions()
{
    DefaultFileNames = new ArrayList()
    {
        "default.html",
        "default.htm"
    }
});
```


## TinyCLR Packages
Install release package from [NuGet](https://www.nuget.org/packages?q=bytewizer.tinyclr) or using the Package Manager Console :
```powershell
PM> Install-Package Bytewizer.TinyCLR.StaticFiles
```