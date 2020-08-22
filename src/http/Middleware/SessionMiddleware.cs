using System;
using System.IO;

using Bytewizer.TinyCLR.Http.Header;
using Bytewizer.TinyCLR.Sockets;

namespace Bytewizer.TinyCLR.Http
{
    public class SessionMiddleware : Middleware
    {
        protected override void Invoke(HttpContext context, RequestDelegate next)
        {
            try
            {
                // empty context
                context.Request.Headers = new HeaderDictionary();
                context.Request.Body = new MemoryStream();
                context.Request.Query = new QueryCollection();

                context.Response.Headers = new HeaderDictionary();
                context.Response.Body = new MemoryStream();
                context.Response.StatusCode = 0;
                
                HttpMessageParser.Decode(context);

                next(context);

                HttpMessageParser.Encode(context);

            }
            catch (Exception ex)
            {
                SendFailure(context, ex);
            }
            finally
            {
                context.Session.Clear();
            }
        }

        private void SendFailure(HttpContext context, Exception ex)
        {

            string response = "HTTP/1.1 500 Internal Server Error\r\nContent-Type: text/html; charset=UTF-8\r\n\r\n" +
                              "<doctype !html><html><head><title>Server Error</title>" +
                              "<style>body { background-color: #111 }" +
                              "h1 { font-size:2cm; text-align: center; color: white;}</style></head>" +
                              "<body><h1>" + DateTime.Now.Ticks.ToString() + "</h1></body></html>";

            context.Session.Write(response);
        }
    }
}