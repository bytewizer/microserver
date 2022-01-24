#if NanoCLR
namespace Bytewizer.NanoCLR.Sockets.Listener
#else
namespace Bytewizer.TinyCLR.Sockets.Listener
#endif
{
    /// <summary>
    /// Represents an options method to configure <see cref="SocketListener"/> specific features.
    /// </summary>
    /// <param name="configure">The <see cref="SocketListener"/> configuration specific features.</param>
    public delegate void SocketListenerOptionsDelegate(SocketListenerOptions configure);
}