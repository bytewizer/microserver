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

### JSON Deserializing
Reads the HTTP content and returns the value that results from deserializing the content as JSON.

```CSharp
endpoints.Map("/json", context =>
{
    // Post a request with the following content as the body and content type of application/json.
    // {"Id": 100,"Suffix": "I","SSN": "939-69-5554","Title": "Mr.","LastName": "Crona","Phone": "(458)-857-7797",
    // "Gender": 0,"FirstName": "Roscoe","MiddleName": "Jerald","Email": "Roscoe@gmail.com","DOB": "2017-01-01T00:00:53.967Z"}
    
    if (context.Request.ReadFromJson(typeof(Person)) is Person person)
    {
        string response = "<doctype !html><html><head><title>Hello, world!" +
        "</title></head><body><h1>" + person.FirstName + " " + person.LastName + "</h1></body></html>";

        context.Response.Write(response);
    }
    else
    {
        context.Response.StatusCode = StatusCodes.Status204NoContent;
    }
});
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
        app.UseMiddleware(typeof(CustomMiddleware));
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

## Developer Exception Page

Captures asynchronous Exception instances from the pipeline and generates HTML error responses. Use the
UseDeveloperException() extension method to render the exception during the development mode. This method
adds middleware into the request pipeline which displays developer-friendly exception detail page. This
middleware should not be used in production.

```CSharp
options.Pipeline(app =>
{
    app.UseDeveloperExceptionPage(); // Should be called first in the pipeline.
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

## Cookie Support

Cookies provide a means in to store user-specific information, such as history or user preferences. A cookie is a small bit of text that accompanies requests and responses as they go between the server and client.

To remove a cookie determine whether the cookie exists, and if so, create a new cookie with the same name and set the max age to 0.

```CSharp
options.Pipeline(app =>
{
    app.UseRouting();
    app.UseEndpoints(endpoints =>
    {
        endpoints.Map("/", context => // Mapped to root url
        {                       
            // Set a response cookie
            context.Response.Cookies.Append("sessionId", "38afes7a8", 86400,
                "bytewizer.local", "device.bytewizer.local", false, false);

            // Get request cookie
            context.Request.Cookies.TryGetValue("sessionId", out string id);

            // Remove/Expire a browser cookie 
            //context.Response.Cookies.Append("sessionId", "38afes7a8", 0,
            //   "bytewizer.local", "device.bytewizer.local", false, false);

            context.Response.Write($"Session Id: {id}", "text/plain");
        });
    });
});
```