using System;
using Bytewizer.TinyCLR.Sockets;

namespace Bytewizer.TinyCLR.Http
{
    public class StatusCodePageMiddleware : Middleware
    {
        private readonly StatusCodePageOptions _options;

        public StatusCodePageMiddleware()
        {
            _options = new StatusCodePageOptions();
        }

        public StatusCodePageMiddleware(StatusCodePageOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            _options = options;
        }

        protected override void Invoke(HttpContext context, RequestDelegate next)
        {
            next(context);

            if (context.Response.StatusCode < 400
                || context.Response.StatusCode >= 600
                || context.Response.ContentLength > 0
                || !string.IsNullOrEmpty(context.Response.ContentType))
            {
                return;
            }

            var statusCode = context.Response.StatusCode;
            var body = BuildResponseBody(statusCode);

            context.Response.Write(body, "text/plain");
        }

        private string BuildResponseBody(int httpStatusCode)
        {
            var reasonPhrase = HttpReasonPhrase.Get(httpStatusCode);
            var separator = string.IsNullOrEmpty(reasonPhrase) ? "" : "; ";
            
            return string.Format($"Status Code: {httpStatusCode}{separator}{reasonPhrase}");
        }
    }
}