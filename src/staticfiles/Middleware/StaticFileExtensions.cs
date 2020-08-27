using System;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Extension methods for the StaticFileMiddleware
    /// </summary>
    public static class StaticFileExtensions 
    {
        /// <summary>
        /// Enables static file serving for the current request path
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseStaticFiles(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware(typeof(StaticFileMiddleware));
        }

        /// <summary>
        /// Enables static file serving with the given options
        /// </summary>
        /// <param name="app"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseStaticFiles(this IApplicationBuilder app, StaticFileOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return app.UseMiddleware(typeof(StaticFileMiddleware), options);
        }
    }
}
