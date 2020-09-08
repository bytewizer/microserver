using Bytewizer.TinyCLR.Sockets;
using System;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Extension methods for adding typed middleware.
    /// </summary>
    public static class UseMiddlewareExtensions
    {
        /// <summary>
        /// Adds a middleware type to the servers request pipeline.
        /// </summary>
        /// <param name="app">The <see cref="ServerOptions"/> instance.</param>
        /// <param name="middleware">The middleware type.</param>
        public static void UseMiddleware(this ServerOptions app, Middleware middleware)
        {
            app.Register(middleware);
        }
    }
}