using System;

namespace Bytewizer.TinyCLR.Http
{
    public static class ControllerExtensions
    {
        public static IApplicationBuilder UseStaticFiles(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware(typeof(ControllerMiddleware));
        }

        public static IApplicationBuilder UseStaticFiles(this IApplicationBuilder app, ControllerOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return app.UseMiddleware(typeof(ControllerMiddleware), options);
        }
    }
}
