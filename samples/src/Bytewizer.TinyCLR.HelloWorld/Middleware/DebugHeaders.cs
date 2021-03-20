using System.Diagnostics;

using Bytewizer.TinyCLR.Http;
using Bytewizer.TinyCLR.Http.Header;

namespace Bytewizer.TinyCLR.HelloWorld
{
    public class DebugHeaders : Middleware
    {
        protected override void Invoke(HttpContext context, RequestDelegate next)
        {
            foreach(HeaderValue header in context.Request.Headers)
            {
                Debug.WriteLine($"{header.Key}={header.Value}");
             }

            // Call the next delegate/middleware in the pipeline
            next(context);
        }
    }
}
