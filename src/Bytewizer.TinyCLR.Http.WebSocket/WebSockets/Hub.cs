using System;

namespace Bytewizer.TinyCLR.Http.WebSockets
{
    /// <summary>
    /// A base class that provides methods to communicate with WebSocket connections that connected to a Hub.
    /// </summary>
    public abstract class Hub : IHub
    {
        private HubCallerContext _hubCallerContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="Hub"/> class.
        /// </summary>
        protected Hub()
        {
        }

        /// <summary>
        /// Gets or sets the <see cref="HubCallerContext"/>.
        /// </summary>
        public HubCallerContext Caller
        {
            get
            {
                if (_hubCallerContext == null)
                {
                    _hubCallerContext = new HubCallerContext();
                }

                return _hubCallerContext;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                _hubCallerContext = value;
            }
        }

        public Clients Clients { get; set; }

        /// <summary>
        /// Called when a new connection is established with the hub.
        /// </summary>
        public virtual void OnConnected()
        {
        }

        /// <inheritdoc/>
        public virtual void OnMessage(WebSocketContext context)
        {
        }

        /// <summary>
        /// Called when a connection with the hub is terminated.
        /// </summary>
        public virtual void OnDisconnected(Exception exception)
        {
        }
    }
}