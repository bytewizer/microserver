using System;

using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Sntp.Internal;
using Bytewizer.TinyCLR.Pipeline.Builder;

namespace Bytewizer.TinyCLR.Sntp
{
    /// <summary>
    /// Extension methods for <see cref="IApplicationBuilder"/>.
    /// </summary>
    public static class SntpMiddlewareExtensions
    {
        /// <summary>
        /// Adds a middleware that 
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder"/> instance.</param>
        /// <param name="loggerFactory">The factory used to create loggers.</param>
        /// <param name="options">The configuration options of <see cref="SntpServer"/> specific features.</param>
        /// <returns>The <see cref="IApplicationBuilder"/> instance.</returns>
        public static void UseSntp(this IApplicationBuilder builder, ILoggerFactory loggerFactory, SntpServerOptions options)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Use(new SntpMiddleware(loggerFactory, options));
        }
    }
}
