using System;
using System.IO;
using Bytewizer.TinyCLR.Http;
using Bytewizer.TinyCLR.Sockets;

namespace Bytewizer.TinyCLR.Sample
{
    public class CustomMiddleware : Middleware
    {
        protected override void Invoke(HttpContext context, RequestDelegate next)
        {

            string response = "<doctype !html><html><head><meta http-equiv='refresh' content='5'><title>Hello, world!</title>" +
                              "<style>body { background-color: #111 } h1 { font-size:2cm; text-align: center; color: white;}</style></head>" +
                              "<body><h1>" + DateTime.Now.Ticks.ToString() + "</h1></body></html>";

            context.Response.Write(response, "text/html");

            next(context);
        }
    }
}