using System;

namespace Bytewizer.TinyCLR.Sockets.Listener
{
    /// <summary>
    /// A delegate which is executed when a remote client has disconnected.
    /// </summary>
    public delegate void DisconnectedHandler(object sender, Exception execption);
}
