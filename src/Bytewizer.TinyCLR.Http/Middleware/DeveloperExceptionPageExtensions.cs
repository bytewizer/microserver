using System;

using Bytewizer.TinyCLR.Sockets;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Extension methods for the <see cref="DeveloperExceptionPageMiddleware"/>.
    /// </summary>
    public static class DeveloperExceptionPageExtensions
    {
        /// <summary>
        /// Captures <see cref="Exception"/> instances from the pipeline and generates error responses.
        /// </summary>
        /// <param name="app">The <see cref="ServerOptions"/> instance this method extends.</param>
        public static void UseDeveloperExceptionPage(this ServerOptions app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.UseMiddleware(new DeveloperExceptionPageMiddleware());
        }

        /// <summary>
        /// Captures <see cref="Exception"/> instances from the pipeline and generates error responses.
        /// </summary>
        /// <param name="app">The <see cref="ServerOptions"/> instance this method extends.</param>
        /// <param name="options">The <see cref="DeveloperExceptionPageOptions"/> used to configure the middleware.</param>
        public static void UseDeveloperExceptionPage(this ServerOptions app, DeveloperExceptionPageOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            app.UseMiddleware(new DeveloperExceptionPageMiddleware(options));
        }
    }
}