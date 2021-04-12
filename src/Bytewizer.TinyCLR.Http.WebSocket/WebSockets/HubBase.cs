using System;

namespace Bytewizer.TinyCLR.Http.WebSockets
{
    /// <summary>
    /// A base class that provides methods to communicate with WebSocket connections that connected to a Hub.
    /// </summary>
    public abstract class HubBase
    {
        private HubCallerContext _hubCallerContext;

        /// <summary>
        /// Gets the <see cref="Http.HttpContext"/> for the executing action.
        /// </summary>
        public HttpContext HttpContext => HubCallerContext.HttpContext;

        /// <summary>
        /// Gets or sets the <see cref="ControllerContext"/>.
        /// </summary>
        public HubCallerContext HubCallerContext
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
    }
}
