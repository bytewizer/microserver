namespace Bytewizer.TinyCLR.Sockets
{
    /// <summary>
    /// Represents a server.
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
