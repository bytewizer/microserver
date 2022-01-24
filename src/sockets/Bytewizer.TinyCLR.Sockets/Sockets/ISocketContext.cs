using Bytewizer.TinyCLR.Pipeline;
using Bytewizer.TinyCLR.Sockets.Channel;

namespace Bytewizer.TinyCLR.Sockets
{
    /// <summary>
    /// An interface for <see cref="SocketContext"/>.
    /// </summary>
    public interface ISocketContext : IContext
    {
        /// <summary>
        /// Gets or sets information about the underlying connection for this request.
        /// </summary>
        SocketChannel Channel { get; set; }
    }
}