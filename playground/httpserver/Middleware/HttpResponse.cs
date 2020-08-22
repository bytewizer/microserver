using System;
using System.Diagnostics;

using Bytewizer.TinyCLR.Http;
using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.Http.Header;

namespace Bytewizer.TinyCLR.WebServer
{
    public class HttpResponse : Middleware
    {
        int clientRequestCount = 1;
        protected override void Invoke(HttpContext context, RequestDelegate next)
        {
            try
            {
                //DebugHeaders(context);

                string response = "HTTP/1.1 200 OK\r\nContent-Type: text/html; charset=UTF-8\r\n\r\n" +
                                  "<doctype !html><html><head><meta http-equiv='refresh' content='50'><title>Hello, world!</title>" +
                                  "<style>body { background-color: #111 }" +
                                  "h1 { font-size:2cm; text-align: center; color: white;}</style></head>" +
                                  "<body><h1>" + DateTime.Now.ToString() + "</h1>" +
                                  "<p style = 'color:red'> Client request count: " + clientRequestCount + "</body></html>";

                context.Response.Write(response);

                next(context);
                clientRequestCount = CountN(clientRequestCount);
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
        private static int CountN(int number)
        {
            ++number;
            return number;
        }
    }
}