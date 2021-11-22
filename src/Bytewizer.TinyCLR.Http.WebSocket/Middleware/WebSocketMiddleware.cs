using System;
using System.Text;
using System.Collections;

using Bytewizer.TinyCLR.Http.Features;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// A middleware to determine if a WebSocket request is processed and 
    /// sets the <see cref="IHttpWebSocketFeature"/> on the <see cref="HttpContext.Features"/>.
    /// </summary>
    public class WebSocketMiddleware : Middleware
    {
        private readonly ArrayList _allowedOrigins;
        private readonly WebSocketOptions _options;

        /// <summary>
        /// A string meaning "All" in Origin headers.
        /// </summary>
        public const string All = "*";

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
            //_allowedOrigins = ParsingHelper.SplitByComma(_options.AllowedOrigins);
        }

        /// <inheritdoc/>
        protected override void Invoke(HttpContext context, RequestDelegate next)
        {
            if (context.Features.Get(typeof(IHttpWebSocketFeature)) == null)
            {
                var webSocketFeature = new HttpWebSocketFeature(context, _options);
                context.Features.Set(typeof(IHttpWebSocketFeature), webSocketFeature);

                if (_options.AllowedOrigins != All)
                {
                    // Check for Origin header
                    var originHeader = context.Request.Headers[HeaderNames.Origin];

                    if (!string.IsNullOrEmpty(originHeader) && webSocketFeature.IsWebSocketRequest)
                    {
                        // Check allowed origins to see if request is allowed
                        if (!_allowedOrigins.Contains(originHeader))
                        {
                            //_logger.LogDebug("Request origin {Origin} is not in the list of allowed origins.", originHeader);
                            context.Response.StatusCode = StatusCodes.Status403Forbidden;
                            return;
                        }
                    }
                }

                next(context);
            }
        }
    }
}