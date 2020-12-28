using System;

using Bytewizer.TinyCLR.Pipeline.Builder;


namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Extension methods for the <see cref="ResourceFileMiddleware"/>.
    /// </summary>
    public static class ResourceFileExtensions 
    {
        /// <summary>
        /// Enables resource file serving for the current request path.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> instance this method extends.</param>
        /// <param name="options">The <see cref="ResourceFileOptions"/> used to configure the middleware.</param>
        public static IApplicationBuilder UseResourceFiles(this IApplicationBuilder app, ResourceFileOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return app.UseMiddleware(typeof(ResourceFileMiddleware), options);
        }
    }
}