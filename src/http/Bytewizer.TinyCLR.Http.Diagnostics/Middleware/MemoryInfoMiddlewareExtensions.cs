using System;

using Bytewizer.TinyCLR.Pipeline.Builder;

namespace Bytewizer.Playground.Sockets
{
    /// <summary>
    /// Extension methods for <see cref="IApplicationBuilder"/>.
    /// </summary>
    public static class MemoryInfoMiddlewareExtensions
    {
        /// <summary>
        /// Adds a middleware that provides device memory information.
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder"/> instance.</param>
        public static void UseMemoryInfo(this IApplicationBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Use(new MemoryInfoMiddleware());
        }
    }
}
