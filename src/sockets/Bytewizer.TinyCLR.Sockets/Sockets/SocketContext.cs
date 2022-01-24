#if NanoCLR
using Bytewizer.NanoCLR.Sockets.Channel;

namespace Bytewizer.NanoCLR.Sockets
#else
using Bytewizer.TinyCLR.Sockets.Channel;

namespace Bytewizer.TinyCLR.Sockets
#endif
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
            Channel?.Client?.Close();
        }

        /// <inheritdoc/>
        public void Clear() 
        {
            Channel?.Clear();
        }
    }
}