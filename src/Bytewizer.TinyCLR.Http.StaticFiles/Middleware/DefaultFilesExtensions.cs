using System;

using Bytewizer.TinyCLR.Pipeline.Builder;

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
        /// <param name="server">The <see cref="IApplicationBuilder"/> instance this method extends.</param>
        public static IApplicationBuilder UseDefaultFiles(this IApplicationBuilder server)
        {
            if (server == null)
            {
                throw new ArgumentNullException(nameof(server));
            }

            return server.UseMiddleware(typeof(DefaultFilesMiddleware));
        }

        /// <summary>
        /// Enables static file serving for the current request path.
        /// </summary>
        /// <param name="server">The <see cref="IApplicationBuilder"/> instance this method extends.</param>
        /// <param name="options">The <see cref="DefaultFilesMiddleware"/> used to configure the middleware.</param>
        public static IApplicationBuilder UseDefaultFiles(this IApplicationBuilder server, DefaultFilesOptions options)
        {
            if (server == null)
            {
                throw new ArgumentNullException(nameof(server));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return server.UseMiddleware(typeof(DefaultFilesMiddleware), options);
        }
    }
}