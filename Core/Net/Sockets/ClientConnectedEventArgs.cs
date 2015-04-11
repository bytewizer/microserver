using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

using Microsoft.SPOT;


namespace MicroServer.Net.Sockets
{
    /// <summary>
    ///     Used by <see cref="SocketListener.OnClientConnected" />.
    /// </summary>
    public class ClientConnectedEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ClientConnectedEventArgs" /> class.
        /// </summary>
        /// <param name="socket">The channel.</param>
        public ClientConnectedEventArgs(Socket socket)
        {
            if (socket == null) throw new ArgumentNullException("socket");

            AllowConnect = true;

            Channel = new SocketChannel();
            ChannelBuffer = new SocketBuffer();
            Channel.Assign(socket);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ClientConnectedEventArgs" /> class.
        /// </summary>
        /// <param name="socket">The channel.</param>
        /// <param name="messageBuffer">The message buffer.</param>
        public ClientConnectedEventArgs(Socket socket, int messageBuffer)
        {
            if (socket == null) throw new ArgumentNullException("socket");

            AllowConnect = true;

            Channel = new SocketChannel();
            ChannelBuffer = new SocketBuffer(messageBuffer);
            Channel.Assign(socket);
        }

        /// <summary>
        ///     Channel for the connected client
        /// </summary>
        public SocketChannel Channel { get; private set; }

        /// <summary>
        ///     Buffer for the connected client
        /// </summary>
        public SocketBuffer ChannelBuffer { get; set; }

        /// <summary>
        ///     Response (only if the client may not connect)
        /// </summary>
        public Stream Response { get; set; }

        /// <summary>
        ///     Determines if the client may connect.
        /// </summary>
        public bool AllowConnect { get; set; }

        /// <summary>
        ///     Cancel connection, will make the listener close it.
        /// </summary>
        public void CancelConnection()
        {
            AllowConnect = false;
        }

        /// <summary>
        ///     Close the listener, but send a response (you are yourself responsible of encoding it to a message)
        /// </summary>
        /// <param name="response">Stream with encoded message (which can be sent as-is).</param>
        public void CancelConnection(Stream response)
        {
            if (response == null) throw new ArgumentNullException("response");
            Response = response;
            AllowConnect = false;
        }
    }
}