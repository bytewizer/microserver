#if NanoCLR
using Bytewizer.NanoCLR.Pipeline;
using Bytewizer.NanoCLR.Sockets.Channel;

namespace Bytewizer.NanoCLR.Sockets
#else
using Bytewizer.TinyCLR.Pipeline;
using Bytewizer.TinyCLR.Sockets.Channel;

namespace Bytewizer.TinyCLR.Sockets
#endif
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