using System;

using Bytewizer.TinyCLR.Pipeline.Builder;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Extension methods for <see cref="IApplicationBuilder"/> to add MVC to the request execution pipeline.
    /// </summary>
    public static class ControllerExtensions
    {
        /// <summary>
        /// Adds MVC to the <see cref="IApplicationBuilder"/> request execution pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/>.</param>
        public static IApplicationBuilder UseMvc(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

           return app.UseMiddleware(typeof(ControllerMiddleware));
        }

        /// <summary>
        /// Adds MVC to the <see cref="IApplicationBuilder"/> request execution pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/>.</param>
        /// <param name="options">The <see cref="ControllerOptions"/> used to configure the middleware.</param>
        public static IApplicationBuilder UseMvc(this IApplicationBuilder app, ControllerOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return app.UseMiddleware(typeof(ControllerMiddleware), options);
        }
    }
}
