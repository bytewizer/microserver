using System;
using System.Collections;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// A middleware to determine if a WebSocket request is processed and 
    /// sets the <see cref="IHttpWebSocketFeature"/> on the <see cref="HttpContext.Features"/>.
    /// </summary>
    public class WebSocketMiddleware : Middleware
    {
        private readonly WebSocketOptions _options;

        /// <summary>
        /// Initializes a default instance of the <see cref="WebSocketMiddleware"/> class.
        /// </summary>
        public WebSocketMiddleware()
        {
            _options = new WebSocketOptions();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebSocketMiddleware"/> class.
        /// </summary>
        public WebSocketMiddleware(WebSocketOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            _options = options;
        }

        /// <inheritdoc/>
        protected override void Invoke(HttpContext context, RequestDelegate next)
        {
            next(context);
        }

        private class UpgradeHandshake
        {
        }
    }
}