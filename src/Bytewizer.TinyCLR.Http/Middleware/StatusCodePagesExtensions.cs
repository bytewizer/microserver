using System;

using Bytewizer.TinyCLR.Pipeline.Builder;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// <see cref="IApplicationBuilder"/> extension methods for the <see cref="StatusCodePagesMiddleware"/>.
    /// </summary>
    public static class StatusCodePagesExtensions
    {
        /// <summary>
        /// Status code middleware with the given options that checks for responses with status codes 
        /// between 400 and 599 that do not have a body.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> instance this method extends.</param>
        public static IApplicationBuilder UseStatusCodePages(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware(typeof(StatusCodePagesMiddleware));
        }

        /// <summary>
        /// Captures <see cref="Exception"/> instances from the pipeline and generates error responses.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> instance this method extends.</param>
        /// <param name="options">The <see cref="StatusCodePagesOptions"/> used to configure the middleware.</param>
        public static IApplicationBuilder UseStatusCodePages(this IApplicationBuilder app, StatusCodePagesOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return app.UseMiddleware(typeof(StatusCodePagesMiddleware), options);
        }
    }
}