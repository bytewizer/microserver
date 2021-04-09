using Bytewizer.TinyCLR.Pipeline;
using Bytewizer.TinyCLR.Sockets.Channel;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Encapsulates all WebSocket-specific information about an individual caller.
    /// </summary>
    public class HubCallerContext : IContext
    {
        /// <summary>
        /// Initializes an instance of the <see cref="HubCallerContext" /> class.
        /// </summary>
        public HubCallerContext() 
        {
        }

        /// <summary>
        /// Gets or sets the object used to manage user session data for this caller.
        /// </summary>
        public SocketChannel Channel { get; set; }

        /// <summary>
        /// Gets the underlying connection id for this caller.
        /// </summary>
        public string ConnectionId => Channel?.Connection?.Id;

        /// <summary>
        /// Aborts the connection underlying this caller.
        /// </summary>
        public void Abort()
        {
            Channel?.Socket?.Close();
        }

        /// <summary>
        /// Closes the connected websocket channel and clears context.
        /// </summary>
        public void Clear()
        {
        
        }
    }
}