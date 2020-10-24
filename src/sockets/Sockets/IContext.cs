using Bytewizer.TinyCLR.Sockets.Channel;

namespace Bytewizer.TinyCLR.Sockets
{
    /// <summary>
    /// An interface for <see cref="Context"/>.
    /// </summary>
    public interface IContext 
    {
        /// <summary>
        /// Gets or sets information about the underlying connection for this request.
        /// </summary>
        SocketChannel Channel { get; set; }
    }
}