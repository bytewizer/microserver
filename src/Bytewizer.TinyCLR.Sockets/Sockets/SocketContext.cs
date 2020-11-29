using Bytewizer.TinyCLR.Sockets.Channel;

namespace Bytewizer.TinyCLR.Sockets
{
    /// <summary>
    /// Encapsulates all socket specific information about an individual request.
    /// </summary>
    public class SocketContext : ISocketContext
    {
        /// <inheritdoc/>
        public SocketChannel Channel { get; set; } = new SocketChannel();

        /// <summary>
        /// Aborts the connection underlying this request.
        /// </summary>
        public void Abort()
        {
            Channel?.Clear();
        }
    }
}