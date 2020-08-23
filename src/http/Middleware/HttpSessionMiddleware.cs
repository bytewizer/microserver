using System;
using System.IO;

using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.Http.Header;
using System.Diagnostics;

namespace Bytewizer.TinyCLR.Http
{
    public class HttpSessionMiddleware : Middleware
    {
        private readonly HttpSessionOptions _options;

        public HttpSessionMiddleware()
        {
            _options = new HttpSessionOptions();
        }

        public HttpSessionMiddleware(HttpSessionOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            _options = options;
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