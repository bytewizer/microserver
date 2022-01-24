using System;

using Bytewizer.TinyCLR.Http.Features;
using Bytewizer.TinyCLR.Http.WebSockets;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Extension methods for <see cref="HttpContext"/> related to websockets.
    /// </summary>
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Extension method for getting the <see cref="WebSocket"/> for the current request.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/> context.</param>
        public static WebSocket GetWebSocket(this HttpContext context)
        {
            return GetWebSocket(context, null);
        }

        /// <summary>
        /// Extension method for getting the <see cref="WebSocket"/> for the current request.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/> context.</param>
        public static WebSocket GetWebSocket(this HttpContext context, string[] subProtocols)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var endpointFeature = (HttpWebSocketFeature)context.Features.Get(typeof(IHttpWebSocketFeature));

            return endpointFeature?.Accept(subProtocols);
        }
    }
}