using System;
using System.Net.Sockets;
using Microsoft.SPOT;

namespace MicroServer.Net.Sockets
{
    /// <summary>
    ///     Event arguments for <see cref="SocketListener.ClientDisconnected" />.
    /// </summary>
    public class ClientDisconnectedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientDisconnectedEventArgs"/> class.
        /// </summary>
        /// <param name="socket">The channel that disconnected.</param>
        /// <param name="exception">The exception that was caught.</param>
        public ClientDisconnectedEventArgs(Socket socket, Exception exception)
        {
            if (socket == null) throw new ArgumentNullException("socket");
            if (exception == null) throw new ArgumentNullException("exception");

            Socket = socket;
            Exception = exception;
        }

        /// <summary>
        /// Channel that was disconnected
        /// </summary>
        public Socket Socket { get; private set; }

        /// <summary>
        /// Exception that was caught (is SocketException if the connection failed or if the remote end point disconnected).
        /// </summary>
        /// <remarks>
        /// <c>SocketException</c> with status <c>Success</c> is created for graceful disconnects.
        /// </remarks>
        public Exception Exception { get; private set; }
    }
}