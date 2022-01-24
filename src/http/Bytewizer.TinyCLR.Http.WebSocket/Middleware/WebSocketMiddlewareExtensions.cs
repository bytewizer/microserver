using System;

using Bytewizer.TinyCLR.Pipeline.Builder;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Extension methods for the <see cref="WebSocketMiddleware"/>.
    /// </summary>
    public static class WebsocketsMiddlewareExtensions
    {
        /// <summary>
        /// Enable enables two-way communication between a client and server.
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder"/> instance this method extends.</param>
        public static IApplicationBuilder UseWebSockets(this IApplicationBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder.UseMiddleware(typeof(WebSocketMiddleware));
        }

        /// <summary>
        /// Enable enables two-way communication between a client and server.
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder"/> instance this method extends.</param>
        /// <param name="options">The <see cref="WebSocketOptions"/> used to configure the middleware.</param>
        public static IApplicationBuilder UseCors(this IApplicationBuilder builder, WebSocketOptions options)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return builder.UseMiddleware(typeof(WebSocketMiddleware), options);
        }
    }
}