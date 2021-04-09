using System;

using Bytewizer.TinyCLR.Pipeline.Builder;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Extension methods for the <see cref="CookiesMiddleware"/>.
    /// </summary>
    public static class CookiesMiddlewareExtensions
    {
        /// <summary>
        /// Enable cross-origin resource sharing (CORS) control capabilities.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> instance this method extends.</param>
        public static IApplicationBuilder UseCookies(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware(typeof(CookiesMiddleware));
        }
    }
}