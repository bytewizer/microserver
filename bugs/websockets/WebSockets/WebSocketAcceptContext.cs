using System;
using System.Collections;
using System.Text;
using System.Threading;

namespace Bytewizer.TinyCLR.Http.WebSockets
{
    /// <summary>
    /// A context for negotiating a websocket upgrade.
    /// </summary>
    public class WebSocketAcceptContext
    {
        /// <summary>
        /// Gets or sets the subprotocol being negotiated.
        /// </summary>
        public virtual string SubProtocol { get; set; }
    }
}
