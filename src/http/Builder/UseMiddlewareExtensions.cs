using System;
using System.Reflection;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Extension methods for adding typed middleware.
    /// </summary>
    public static class UseMiddlewareExtensions
    {
        /// <summary>
        /// Adds a middleware type to the application's request pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> instance.</param>
        /// <param name="middleware">The middleware type.</param>
        /// <param name="args">The arguments to pass to the middleware type instance's constructor.</param>
        /// <returns>The <see cref="IApplicationBuilder"/> instance.</returns>
        public static IApplicationBuilder UseMiddleware(this IApplicationBuilder app, Type middleware, params object[] args)
        {
            var methods = middleware.GetMethods(BindingFlags.Instance | BindingFlags.Public);

            return app.Use(next =>
            {

            });
        }
    }
}
