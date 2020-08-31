using System;

using Bytewizer.TinyCLR.Sockets;

namespace Bytewizer.TinyCLR.Http
{
    public static class RoutingExtensions
    {
        public static void UseRouting(this ServerOptions app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.UseMiddleware(new RoutingMiddleware());
        }

        public static void UseRouting(this ServerOptions app, RoutingOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            app.UseMiddleware(new RoutingMiddleware(options));
        }
    }
}
