using System;
using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Pipeline.Builder;

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
        /// <param name="app">The <see cref="IApplicationBuilder"/> instance this method extends.</param>
        public static IApplicationBuilder UseStaticFiles(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware(typeof(StaticFileMiddleware));
        }

        /// <summary>
        /// Enables static file serving for the current request path.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> instance this method extends.</param>
        /// <param name="loggerFactory">The factory used to create loggers.</param>
        public static IApplicationBuilder UseStaticFiles(this IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            return app.UseMiddleware(typeof(StaticFileMiddleware), loggerFactory, new StaticFileOptions());
        }

        /// <summary>
        /// Enables static file serving for the current request path.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> instance this method extends.</param>
        /// <param name="options">The <see cref="StaticFileOptions"/> used to configure the middleware.</param>
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