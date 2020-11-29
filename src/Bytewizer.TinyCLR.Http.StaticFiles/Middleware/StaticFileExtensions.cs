using System;

using Bytewizer.TinyCLR.Sockets;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Extension methods for the <see cref="StaticFileMiddleware"/>.
    /// </summary>
    public static class StaticFileExtensions 
    {
        /// <summary>
        /// Enables static file serving for the current request path.
        /// </summary>
        /// <param name="options">The <see cref="ServerOptions"/> instance this method extends.</param>
        public static void UseStaticFiles(this ServerOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            options.UseMiddleware(new StaticFileMiddleware());
        }

        /// <summary>
        /// Enables static file serving for the current request path.
        /// </summary>
        /// <param name="app">The <see cref="ServerOptions"/> instance this method extends.</param>
        /// <param name="options">The <see cref="StaticFileOptions"/> used to configure the middleware.</param>
        public static void UseStaticFiles(this ServerOptions app, StaticFileOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            app.UseMiddleware(new StaticFileMiddleware(options));
        }
    }
}