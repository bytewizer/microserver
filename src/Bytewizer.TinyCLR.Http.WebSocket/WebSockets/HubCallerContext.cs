using Bytewizer.TinyCLR.Pipeline;
using Bytewizer.TinyCLR.Sockets.Channel;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Encapsulates all WebSocket-specific information about an individual caller.
    /// </summary>
    public class HubCallerContext 
    {
        /// <summary>
        /// Initializes an instance of the <see cref="HubCallerContext" /> class.
        /// </summary>
        public HubCallerContext()
        {
        }

        /// <summary>
        /// Initializes an instance of the <see cref="HubCallerContext" /> class.
        /// </summary>
        public HubCallerContext(HttpContext context) 
        {
            HttpContext = context;
        }

        /// <summary>
        /// Gets or sets the <see cref="Http.HttpContext"/> for the current request.
        /// </summary>

        public HttpContext HttpContext { get; private set; }
    }
}