using System;

namespace Bytewizer.TinyCLR.Http
{
    public static class RoutingExtensions
    {
        public static IApplicationBuilder UseStaticFiles(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware(typeof(RoutingMiddleware));
        }

        public static IApplicationBuilder UseStaticFiles(this IApplicationBuilder app, RoutingOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return app.UseMiddleware(typeof(RoutingMiddleware), options);
        }
    }
}
