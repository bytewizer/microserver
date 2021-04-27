# HTTP Web Server

## Custom Endpoint Routing
```CSharp
var server = new HttpServer(options =>
{
    options.Pipeline(app =>
    {
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.Map("/money", context => 
            {
                context.Response.Write("Show me the money!"); 
            });
        });
    });
});
server.Start();
```

### Send File Response
Sends a given file from a sd card or usb drive.

```CSharp
endpoints.Map("/image", context =>
{
    context.Response.SendFile(@"\img\wave.jpg");
});
```
### Header Redirects
Redirects a request to a new URL and specifies the new URL.

```CSharp
endpoints.Map("/", context =>
{
    context.Response.Redirect("/page", true, true);
});
```

## Custom Middleware
Middleware is generally encapsulated in a class and exposed with an extension method. Every request sent runs through the pipeline of configured middleware before it is processed to generate a response. Each middleware can be programmed to perform some work in two distinct steps: before and after the request gets processed. middleware components are invoked in the order they have been registered. The figure below shows the overall diagram.

![Middleware Pipeline](../../images/pipeline.jpeg)

```CSharp
public class CustomMiddleware : Middleware
{
    protected override void Invoke(HttpContext context, RequestDelegate next)
    {
        string response = "Hello from the middle of knowhere!";

        context.Response.Write(response);

        // Call the next delegate/middleware in the pipeline
        next(context);
    }
}
```

### Middleware Extension Method
The following extension method exposes the middleware through UseMiddleware():

```CSharp
public static class CustomMiddlewareExtensions
{
    public static void UseCustomMiddleware(this IApplicationBuilder app)
    {
        if (app == null)
        {
            throw new ArgumentNullException(nameof(app));
        }

        app.SetProperty("key", "custom object/setting");
        app.UseMiddleware(new CustomMiddleware());
    }
}
```
The following code calls the middleware from HttpServer options:

```CSharp
options.Pipeline(app =>
{
    app.UseRouting();
    app.UseCustomMiddleware();
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers(); 
    });
});
```

## Status Code Pages

Adds a StatusCodePages middleware with a default response handler that checks for responses with
status codes between 400 and 599 that do not have a body.

```CSharp
options.Pipeline(app =>
{
    app.UseStatusCodePages(new StatusCodePagesOptions
    {
        Handle = context =>
        {
            var response = context.Response;
            if (response.StatusCode < 500)
            {
                response.Write($"Client error ({response.StatusCode})");
            }
            else
            {
                response.Write($"Server error ({response.StatusCode})");
            }
        }
    });
});
```