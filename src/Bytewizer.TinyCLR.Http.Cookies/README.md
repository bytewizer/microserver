# Cookies

Enables the HTTP Cookie and Set-Cookie header fields. Cookies provide a means in to store user-specific information, such as history or user preferences. A cookie is a small bit of text that accompanies requests and responses as they go between the server and client. To remove a cookie determine whether the cookie exists, and if so, create a new cookie with the same name and set the max age to 0.

## Simple Cookie Example
```CSharp
var server = new HttpServer(options =>
{
    options.Pipeline(app =>
    {
        app.UseRouting();
        app.UseCookies(); 
        app.UseEndpoints(endpoints =>
        {
            endpoints.Map("/", context =>
            {
                var cookies = context.GetCookies();                 
                cookies.TryGetValue("sid", out string id);
                            
                var responseCookies = context.GetResponseCookies();
                responseCookies.Append("sux", "45a4ffgra8");

                responseCookies.Append("sid", "38afes7a9", 86400,
                    "/", "192.168.1.145", false, false);

                // Remove/Expire a browser cookie 
                responseCookies.Append("sux", "45a4ffgra8", 0, // set max age = 0
                    "/", "192.168.1.145", false, false);

                context.Response.Write($"Session Id: {id}");
            });
        });
    });
});
server.Start();
```

## TinyCLR Packages
Install release package from [NuGet](https://www.nuget.org/packages?q=bytewizer.tinyclr) or using the Package Manager Console :
```powershell
PM> Install-Package Bytewizer.TinyCLR.Http.Cookies
```

## RFC - Related Request for Comments 
- [RFC 6265, section 5.4: Cookie](https://tools.ietf.org/html/rfc6265#section-5.4)