using System;

using Bytewizer.TinyCLR.Pipeline.Builder;

namespace Bytewizer.TinyCLR.Terminal
{
    /// <summary>
    /// <see cref="IApplicationBuilder"/> extension methods for the <see cref="EndpointMiddleware"/>.
    /// </summary>
    public static class EndpointExtensions
    {
        /// <summary>
        /// Adds routing middleware features supporting endpoint routing.
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder"/> instance this method extends.</param>
        /// <param name="serverCommand">The instance this method extends.</param>
        public static IApplicationBuilder UseEndpoint(this IApplicationBuilder builder, ServerCommand serverCommand)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder.Use(typeof(EndpointMiddleware), serverCommand);
        }
    }
}
