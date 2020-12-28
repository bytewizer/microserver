using System.Net.Sockets;

namespace Bytewizer.TinyCLR.Sockets.Listener
{
    /// <summary>
    /// A delegate which is executed when a client has connected.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="socket">The socket for the connected end point.</param>
    public delegate void ConnectedHandler(object sender, Socket socket);
}