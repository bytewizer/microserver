using System;

using Bytewizer.TinyCLR.Sockets;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Extension methods for the <see cref="StatusCodePagesMiddleware"/>.
    /// </summary>
    public static class StatusCodePagesExtensions
    {
        /// <summary>
        /// Status code middleware with the given options that checks for responses with status codes 
        /// between 400 and 599 that do not have a body.
        /// </summary>
        /// <param name="app">The <see cref="ServerOptions"/> instance this method extends.</param>
        public static void UseStatusCodePages(this ServerOptions app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.UseMiddleware(new StatusCodePagesMiddleware());
        }

        /// <summary>
        /// Captures <see cref="Exception"/> instances from the pipeline and generates error responses.
        /// </summary>
        /// <param name="app">The <see cref="ServerOptions"/> instance this method extends.</param>
        /// <param name="options">The <see cref="StatusCodePagesOptions"/> used to configure the middleware.</param>
        public static void UseStatusCodePages(this ServerOptions app, StatusCodePagesOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            app.UseMiddleware(new StatusCodePagesMiddleware(options));
        }
    }
}