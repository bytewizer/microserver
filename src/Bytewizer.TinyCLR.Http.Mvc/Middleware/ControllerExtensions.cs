using System;

using Bytewizer.TinyCLR.Sockets;

namespace Bytewizer.TinyCLR.Http
{
    public static class ControllerExtensions
    {
        public static void UseMvc(this ServerOptions app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.UseMiddleware(new ControllerMiddleware());
        }

        public static void UseMvc(this ServerOptions app, ControllerOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            app.UseMiddleware(new ControllerMiddleware(options));
        }
    }
}
