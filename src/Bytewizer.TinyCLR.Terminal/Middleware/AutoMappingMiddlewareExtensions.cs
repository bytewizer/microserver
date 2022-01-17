using System;

using Bytewizer.TinyCLR.Pipeline.Builder;

namespace Bytewizer.TinyCLR.Terminal
{
    /// <summary>
    /// <see cref="IApplicationBuilder"/> extension methods for the <see cref="AutoMappingMiddleware"/>.
    /// </summary>
    public static class AutoMappingMiddlewareExtensions
    {
        /// <summary>
        /// Adds auto mapping of endpoints for command actions from assembly reflection.
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder"/> instance this method extends.</param>
        public static IApplicationBuilder UseAutoMapping(this IApplicationBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder.Use(new AutoMappingMiddleware());
        }
    }
}
