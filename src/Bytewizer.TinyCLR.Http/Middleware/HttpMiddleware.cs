using System.IO;
using System.Diagnostics;

using Bytewizer.TinyCLR.Pipeline;
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
            HttpMessageParser.Decode(context);

            DebugHeaders(context);
            DebugBody(context);

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

        private void DebugBody(HttpContext context)
        {
            if (context.Request.Body.Length > 0)
            {
                Debug.WriteLine(string.Empty);
                Debug.WriteLine("---------- Body Content ----------");
                var reader = new StreamReader(context.Request.Body);
                while (reader.Peek() != -1)
                {
                    var line = reader.ReadLine();
                    Debug.WriteLine(line);
                }
                context.Request.Body.Position = 0;
            }
        }
    }
}