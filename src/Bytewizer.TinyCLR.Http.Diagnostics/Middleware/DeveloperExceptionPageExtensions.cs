using System;

using Bytewizer.TinyCLR.Pipeline.Builder;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// <see cref="IApplicationBuilder"/> extension methods for the <see cref="DeveloperExceptionPageMiddleware"/>.
    /// </summary>
    public static class DeveloperExceptionPageExtensions
    {
        /// <summary>
        /// Captures <see cref="Exception"/> instances from the pipeline and generates error responses.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> instance this method extends.</param>
        public static IApplicationBuilder UseDeveloperExceptionPage(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware(typeof(DeveloperExceptionPageMiddleware));
        }

        /// <summary>
        /// Captures <see cref="Exception"/> instances from the pipeline and generates error responses.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> instance this method extends.</param>
        /// <param name="options">The <see cref="DeveloperExceptionPageOptions"/> used to configure the middleware.</param>
        public static IApplicationBuilder UseDeveloperExceptionPage(this IApplicationBuilder app, DeveloperExceptionPageOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return app.UseMiddleware(typeof(DeveloperExceptionPageMiddleware), options);
        }
    }
}