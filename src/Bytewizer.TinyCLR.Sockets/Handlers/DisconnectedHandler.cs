using System;

namespace Bytewizer.TinyCLR.Sockets.Handlers
{
    /// <summary>
    /// A delegate which is executed when a client has disconnected.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="execption">The <see cref="Exception"/> for the error.</param>
    public delegate void DisconnectedHandler(object sender, Exception execption);
}
