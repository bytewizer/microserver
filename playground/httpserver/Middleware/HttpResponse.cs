using System;
using System.Diagnostics;

using Bytewizer.TinyCLR.Http;
using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.Http.Header;

namespace Bytewizer.TinyCLR.WebServer
{
    public class HttpResponse : Middleware
    {
        protected override void Invoke(HttpContext context, RequestDelegate next)
        {
            try
            {
                DebugHeaders(context);

                string response = "<doctype !html><html><head><meta http-equiv='refresh' content='5'><title>Hello, world!</title>" +
                                  "<style>body { background-color: #111 } h1 { font-size:2cm; text-align: center; color: white;}</style></head>" +
                                  "<body><h1>" + DateTime.Now.Ticks.ToString() + "</h1></body></html>";

                context.Response.Write(response);

                next(context);
            }
            catch (Exception ex)
            {
                // try to manage all unhandled exceptions in the pipeline
                Debug.WriteLine($"Unhandled exception message: { ex.Message } StackTrace: {ex.StackTrace}");
            }
        }

        private void DebugHeaders(HttpContext context)
        {
            Debug.WriteLine(string.Empty);
            
            foreach (HeaderValue header in context.Request.Headers)
            {
                Debug.WriteLine($"{header.Key}={header.Value}");
            }
        }
    }
}