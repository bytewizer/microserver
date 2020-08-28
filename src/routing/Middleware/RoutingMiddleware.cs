using System;

using Bytewizer.TinyCLR.Sockets;

namespace Bytewizer.TinyCLR.Http
{
    public class RoutingMiddleware : Middleware
    {
        private readonly RoutingOptions _options;

        public RoutingMiddleware()
            : this (new RoutingOptions())
        { }

        public RoutingMiddleware(RoutingOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            _options = options;
        }

        protected override void Invoke(HttpContext context, RequestDelegate next)
        {
            context.Request.RouteValues = new RouteDictionary
            {
                { "default", new string[] { context.Request.Path } }
            };
            
            next(context);
        }
    }
}