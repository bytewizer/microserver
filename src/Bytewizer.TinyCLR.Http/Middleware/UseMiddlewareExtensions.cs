using System;

using Bytewizer.TinyCLR.Pipeline;
using Bytewizer.TinyCLR.Pipeline.Builder;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Extension methods for adding typed middleware.
    /// </summary>
    public static class UseMiddlewareExtensions
    {
        /// <summary>
        /// Adds a <see cref="IMiddleware"/> type to the servers request pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> instance.</param>
        /// <param name="middleware">The middleware type.</param>
        public static IApplicationBuilder UseMiddleware(this IApplicationBuilder app, Type middleware)
        {
            var instance = (Middleware)Activator.CreateInstance(middleware);
            return app.Use(instance);
        }

        /// <summary>
        /// Adds a <see cref="IMiddleware"/> type to the servers request pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> instance.</param>
        /// <param name="middleware">The middleware type.</param>
        /// <param name="args">The arguments to pass to the middleware type instance's constructor.</param>
        public static IApplicationBuilder UseMiddleware(this IApplicationBuilder app, Type middleware, params object[] args)
        {
            var instance = (Middleware)Activator.CreateInstance(middleware, args);
            return app.Use(instance);
        }
    }
}