using System.Net.Sockets;

namespace Bytewizer.TinyCLR.Sockets.Listener
{
    /// <summary>
    /// A delegate which is executed when a client has connected.
    /// </summary>
    public delegate void ConnectedHandler(object sender, Socket socket);
}