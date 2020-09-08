using System;

using Bytewizer.TinyCLR.Sockets;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Extension methods for the <see cref="DefaultFilesMiddleware"/>.
    /// </summary>
    public static class DefaultFilesExtensions 
    {
        /// <summary>
        /// Enables static file serving for the current request path.
        /// </summary>
        /// <param name="app">The <see cref="ServerOptions"/> instance this method extends.</param>
        public static void UseDefaultFiles(this ServerOptions app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.UseMiddleware(new DefaultFilesMiddleware());
        }

        /// <summary>
        /// Enables static file serving for the current request path.
        /// </summary>
        /// <param name="app">The <see cref="ServerOptions"/> instance this method extends.</param>
        /// <param name="options">The <see cref="DefaultFilesMiddleware"/> used to configure the middleware.</param>
        public static void UseDefaultFiles(this ServerOptions app, DefaultFilesOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            app.UseMiddleware(new DefaultFilesMiddleware(options));
        }
    }
}