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
        /// <param name="server">The <see cref="ServerOptions"/> instance this method extends.</param>
        public static void UseDefaultFiles(this ServerOptions server)
        {
            if (server == null)
            {
                throw new ArgumentNullException(nameof(server));
            }

            server.UseMiddleware(new DefaultFilesMiddleware());
        }

        /// <summary>
        /// Enables static file serving for the current request path.
        /// </summary>
        /// <param name="server">The <see cref="ServerOptions"/> instance this method extends.</param>
        /// <param name="options">The <see cref="DefaultFilesMiddleware"/> used to configure the middleware.</param>
        public static void UseDefaultFiles(this ServerOptions server, DefaultFilesOptions options)
        {
            if (server == null)
            {
                throw new ArgumentNullException(nameof(server));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            server.UseMiddleware(new DefaultFilesMiddleware(options));
        }
    }
}