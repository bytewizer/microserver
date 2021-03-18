using System;

using Bytewizer.TinyCLR.Http;

namespace Bytewizer.Playground.Http
{
    public class ResponseMiddleware : Middleware
    {
        protected override void Invoke(HttpContext context, RequestDelegate next)
        {
            if (context.Request.Method == HttpMethods.Get
                && context.Request.Path == "/")
            {
                string response = "<doctype !html><html><head><meta http-equiv='refresh' content='5'><title>Hello, world!</title>" +
                                  "<style>body { background-color: #7e00cc } h1 { font-size:2cm; text-align: center; color: white;}</style></head>" +
                                  "<body><h1>" + DateTime.Now.Ticks.ToString() + "</h1></body></html>";

                // send the response to browser
                context.Response.Write(response);
            }

            next(context);
        }
    }
}