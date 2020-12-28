namespace Bytewizer.TinyCLR.Sockets
{
    /// <summary>
    /// An interface for <see cref="SocketService"/>.
    /// </summary>
    public interface IServer
    {
        /// <summary>
        /// Start the server accepting incoming requests.
        /// </summary>
        bool Start();

        /// <summary>
        /// Stop processing requests and gracefully shut down the server if possible.
        /// </summary>
        bool Stop();
    }
}
