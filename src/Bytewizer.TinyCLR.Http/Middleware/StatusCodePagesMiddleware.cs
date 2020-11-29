using System;

using Bytewizer.TinyCLR.Pipeline;

namespace Bytewizer.TinyCLR.Http
{
    public class StatusCodePagesMiddleware : Middleware
    {
        private readonly StatusCodePagesOptions _options;

        public StatusCodePagesMiddleware()
        {
            _options = new StatusCodePagesOptions();
        }

        public StatusCodePagesMiddleware(StatusCodePagesOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            _options = options;

            if (_options.Handle == null)
            {
                throw new ArgumentException("Missing options.HandleAsync implementation.");
            }
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

            _options.Handle(context);
        }
    }
}