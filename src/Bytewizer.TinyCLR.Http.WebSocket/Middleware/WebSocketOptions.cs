using System;
using System.Collections;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Options for configuring the <see cref="WebSocketMiddleware"/>.
    /// </summary>
    public class WebSocketOptions
    {
        /// <summary>
        /// Create an instance with the default options settings.
        /// </summary>
        public WebSocketOptions()
        {
            KeepAliveInterval = TimeSpan.FromMinutes(2);
            ReceiveBufferSize = 4 * 1024;
            AllowedOrigins = "*";
        }

        /// <summary>
        /// Gets or sets the frequency at which to send Ping/Pong keep-alive control frames.
        /// The default is two minutes.
        /// </summary>
        public TimeSpan KeepAliveInterval { get; set; }

        /// <summary>
        /// Gets or sets the size of the protocol buffer used to receive and parse frames.
        /// The default is 4kb.
        /// </summary>
        public int ReceiveBufferSize { get; set; }

        /// <summary>
        /// Set the Origin header values allowed for WebSocket requests to prevent Cross-Site WebSocket Hijacking.
        /// By default all Origins are allowed.
        /// </summary>
        public string AllowedOrigins { get; }
    }
}