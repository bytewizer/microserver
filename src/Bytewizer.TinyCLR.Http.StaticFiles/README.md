# Static File Handling
Static files when found are served and sent back as-is to the client. 

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
            options.UseFileServer();
        });
        server.Start();
    }
}
```

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
            // UseDefaultFiles() must be called before UseStaticFile().
            options.UseDefaultFiles();
            options.UseStaticFiles();
        });
        server.Start();
    }
}
```

To set default.html as a default page displayed on root access. Use DefaultFilesOptions method as shown below.
```CSharp
options.UseDefaultFiles(new DefaultFilesOptions()
{
    DefaultFileNames = new ArrayList()
    {
        "default.html",
        "default.htm"
    }
});
```

## Embedded Resource Files

The UseResourceFiles() middleware is used to access files embedded within assemblies.

```CSharp
class Program
{    
    static void Main(string[] args)
    { 
        var server = new HttpServer(options =>
        {
            options.UseResourceFiles(new ResourceFileOptions()
                {
                    // Resource manager must be included as an option along with a list of resoruces
                    ResourceManager = Resources.ResourceManager,
                    Resources = new Hashtable()
                    {
                        { "/resource.html", Resources.StringResources.Resource }
                    }
                });
        });
        server.Start();
    }
}
```