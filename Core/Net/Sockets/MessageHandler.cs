using System;

namespace MicroServer.Net.Sockets
{
    /// <summary>
    /// <see cref="SocketChannel"/> has sent or received a message.
    /// </summary>
    /// <param name="sender">Channel that did the work</param>
    /// <param name="args">Socket event arguments.</param>
    /// <remarks>We uses delegates instead of regular events to make sure that there are only one subscriber and that it's configured once.</remarks>
    public delegate void MessageHandler(object sender, SocketEventArgs args);
}