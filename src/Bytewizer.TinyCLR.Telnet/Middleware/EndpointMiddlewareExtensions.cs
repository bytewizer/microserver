using System;
using Bytewizer.TinyCLR.Pipeline.Builder;

namespace Bytewizer.TinyCLR.Telnet
{
    /// <summary>
    /// <see cref="IApplicationBuilder"/> extension methods for the <see cref="EndpointMiddleware"/>.
    /// </summary>
    public static class EndpointMiddlewareExtensions
    {
        /// <summary>
        /// Adds command routing middleware features.
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder"/> instance this method extends.</param>
        public static IApplicationBuilder UseCommands(this IApplicationBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder.Use(new EndpointMiddleware());
        }   
    }
}