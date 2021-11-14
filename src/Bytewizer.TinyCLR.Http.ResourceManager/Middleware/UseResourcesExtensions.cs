using System;
using System.Resources;

using Bytewizer.TinyCLR.Pipeline.Builder;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Extension methods for <see cref="IApplicationBuilder"/>.
    /// </summary>
    public static class UseResourcesExtensions
    {
        /// <summary>
        /// Adds a middleware that includes a <see cref="ResourceManager"/> in the request.
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder"/> instance.</param>
        /// <param name="resourceManager">The <see cref="ResourceManager"/> for configuring the middleware.</param>
        /// <returns>The <see cref="IApplicationBuilder"/> instance.</returns>
        public static IApplicationBuilder UseResources(this IApplicationBuilder builder, ResourceManager resourceManager)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder.UseMiddleware(typeof(ResourceMiddleware), resourceManager);
        }
    }
}