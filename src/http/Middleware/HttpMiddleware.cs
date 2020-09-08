using System;
using System.IO;
using System.Diagnostics;

using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.Http.Header;

namespace Bytewizer.TinyCLR.Http
{
    public class HttpMiddleware : Middleware
    {
        public HttpMiddleware()
        {
        }

        protected override void Invoke(HttpContext context, RequestDelegate next)
        {
            // empty context
            context.Request.Headers = new HeaderDictionary();
            context.Request.Body = new MemoryStream();
            context.Request.Query = new QueryCollection();

            context.Response.Headers = new HeaderDictionary();
            context.Response.Body = new MemoryStream();
            context.Response.StatusCode = 0;

            HttpMessageParser.Decode(context);

            DebugHeaders(context);

            if (context.Request.Body.Length > 0)
            {
                Debug.WriteLine("---------- Body Content ----------");
                var reader = new StreamReader(context.Request.Body);
                while (reader.Peek() != -1)
                {
                    var line = reader.ReadLine();
                    Debug.WriteLine(line);
                }
                context.Request.Body.Position = 0;
            }

            next(context);

            HttpMessageParser.Encode(context);
        }


 

        private void DebugHeaders(HttpContext context)
        {
            Debug.WriteLine(string.Empty);

            foreach (HeaderValue header in context.Request.Headers)
            {
                Debug.WriteLine($"{header.Key}: {header.Value}");
            }
        }
    }
}