using System;

using Bytewizer.TinyCLR.Sockets;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Extension methods for the <see cref="StaticFileMiddleware"/>.
    /// </summary>
    public static class FileServerExtensions 
    {
        /// <summary>
        /// Enable all static file middleware for the current request path in the root directory.
        /// </summary>
        /// <param name="app">The <see cref="ServerOptions"/> instance this method extends.</param>
        public static void UseFileServer(this ServerOptions app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
        }
    }
}