using System;

#if NanoCLR
namespace Bytewizer.NanoCLR.Sockets
#else
namespace Bytewizer.TinyCLR.Sockets
#endif
{
    /// <summary>
    /// A delegate which is executed when a client has disconnected.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="execption">The <see cref="Exception"/> for the error.</param>
    public delegate void DisconnectedHandler(object sender, Exception execption);
}
