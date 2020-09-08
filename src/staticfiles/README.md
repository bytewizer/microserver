# Static Files Examples

## UseStaticFiles() - Static Files 

Static files (js, css, html etc) when found are served, no further processing is required, and sends the file back as-is to the client. Its the responsibility of UseStaticFiles() middleware is to look for a file path (for example images/image.jpeg) and serve content from this folder.

The UseDefaultFiles() middleware serves the following files on the root request. UseDefaultFiles() is a URL rewriter that doesn't actually serve the file. UseDefaultFiles() must be called before UseStaticFiles() to serve the default file.

1.  Index.html
2.  Index.htm

```CSharp
class Program
{
    static void Main(string[] args)
    {
        // Storage devices must be mounted to access sd cards
        var sd = StorageController.FromName(SC20100.StorageController.SdCard);
        FileSystem.Mount(sd.Hdc);
        
        var server = new SocketServer(options =>
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
## UseFileServer() - Static File Server
UseFileServer() combines the functionality of UseStaticFiles() and UseDefaultFiles().

## UseResourceFiles() - Resource Files

```CSharp
class Program
{    
    static void Main(string[] args)
    { 
        var server = new SocketServer(options =>
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