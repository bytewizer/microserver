using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;

using MicroServer.Logging;

namespace MicroServer.Net.Sockets
{
    /// <summary>
    /// Represents a socket connection between two end points.
    /// </summary>
    public class SocketChannel : IDisposable
    {
        #region Private Properties

        private Socket _socket;
        private MessageHandler _messageReceived;
        private MessageHandler _messageSent;
        private DisconnectHandler _clientDisconnected;

        private bool _connected = false;

        #endregion Private Properties

        #region Public Properties

        /// <summary>
        ///     Gets address of the connected end point.
        /// </summary>
        public EndPoint RemoteEndpoint { get; set; }

        /// <summary>
        ///     Gets socket for the connected end point.
        /// </summary>
        public Socket Socket
        {
            get { return _socket; }
        }

        /// <summary>
        ///     Identity of this channel
        /// </summary>
        /// <remarks>
        ///     Must be unique within a server.
        /// </remarks>
        public string ChannelId { get; private set; }

        /// <summary>
        ///     Can be used to store information in the channel so that you can access it at later requests.
        /// </summary>
        /// <remarks>
        ///     <para>All data is lost when the channel is closed.</para>
        /// </remarks>
        public Byte[] Data { get; set; }

        /// <summary>
        /// Gets if channel is connected
        /// </summary>
        public bool IsConnected
        {
            get { return _connected; }
        }

        /// <summary>
        ///     Channel got disconnected
        /// </summary>
        public DisconnectHandler Disconnected
        {
            get { return _clientDisconnected; }

            set
            {
                if (value == null)
                    _clientDisconnected = (x, e) => { };
                else
                    _clientDisconnected = value;
            }
        }

        /// <summary>
        ///     Channel received a new message
        /// </summary>
        public MessageHandler MessageReceived
        {
            get { return _messageReceived; }
            set
            {
                if (value == null)
                    throw new ArgumentException("You must have a MessageReceived delegate");

                _messageReceived = value;
            }
        }

        /// <summary>
        ///     Channel have sent a message
        /// </summary>
        public MessageHandler MessageSent
        {
            get { return _messageSent; }

            set
            {
                if (value == null)
                {
                    _messageSent = (x, y) => { };
                    return;
                }

                _messageSent = value;
            }
        }

        /// <summary>
        ///     The channel failed to complete an IO operation
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         The handler MUST close the connection once a reply have been sent.
        ///     </para>
        /// </remarks>
        public ErrorHandler ChannelFailure { get; set; }

        #endregion Public Properties

        #region Constructors / Deconstructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SocketChannel" /> class.
        /// </summary>
        public SocketChannel()
        {
            ChannelId = Guid.NewGuid().ToString();
            RemoteEndpoint = new IPEndPoint(0, 0);

            _messageReceived = (channel, message) => { };
            _messageSent = (channel, message) => { };
            _clientDisconnected = (channel, exception) => { };

            ChannelFailure = (channel, error) => HandleDisconnect(SocketError.ProtocolNotSupported);
        }

        /// <summary>
        /// Handles object cleanup for GC finalization.
        /// </summary>
        ~SocketChannel()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Handles object cleanup.
        /// </summary>
        public void Dispose()
        {          
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Handles object cleanup
        /// </summary>
        /// <param name="disposing">True if called from Dispose(); false if called from GC finalization.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _socket.Close();
                _socket = null;
            }
        }
        
        #endregion  Constructors / Deconstructors

        #region Methods

        /// <summary>
        ///     Assign a socket to this channel
        /// </summary>
        /// <param name="socket">Connected socket for channel.</param>
        /// <remarks>
        ///     The channel will start receive new messages as soon as you've called assign.
        ///     <para>
        ///         You must have specified a <see cref="MessageReceived" /> delegate first.
        ///     </para>
        /// </remarks>
        public void Assign(Socket socket)
        {
            if (socket == null) throw new ArgumentNullException("socket");
            _socket = socket;
            _connected = true;
        }

        /// <summary>
        ///     Send a new message to a connected socket.
        /// </summary>
        /// <param name="message">An array of type byte that contains the data to be sent.</param>
        /// <param name="endPoint">An EndPoint that represents the remote device. </param>
        public int SendTo(byte[] message, EndPoint endPoint)
        {
            int bytesSent = 0;
            try
            {
                bytesSent = _socket.SendTo(message, endPoint);
                _messageSent(this, new SocketEventArgs(_socket));
            }
            catch (Exception ex)
            {
                ChannelFailure(this, ex);
            }
            return bytesSent;
        }

        /// <summary>
        ///     Send a new message to a connected socket.
        /// </summary>
        /// <param name="message">An array of type byte that contains the data to be sent.</param>
        /// <param name="socketFlags">A bitwise combination of the SocketFlags values.</param>
        /// <param name="endPoint">An EndPoint that represents the remote device. </param>
        public int SendTo(byte[] message, SocketFlags socketFlags, EndPoint endPoint)
        {
            int bytesSent = 0;
            try
            {
                bytesSent = _socket.SendTo(message, socketFlags, endPoint);
                _messageSent(this, new SocketEventArgs(_socket));
            }
            catch (Exception ex)
            {
                ChannelFailure(this, ex);
            }
            return bytesSent;
        }

        /// <summary>
        ///     Send a new message to a connected socket.
        /// </summary>
        /// <param name="message">An array of type byte that contains the data to be sent.</param>
        /// <param name="size">The number of bytes to send.</param>
        /// <param name="socketFlags">A bitwise combination of the SocketFlags values.</param>
        /// <param name="endPoint">An EndPoint that represents the remote device. </param>
        public int SendTo(byte[] message, int size, SocketFlags socketFlags, EndPoint endPoint)
        {
            int bytesSent = 0;
            try
            {
                bytesSent = _socket.SendTo(message, size, socketFlags, endPoint);
                _messageSent(this, new SocketEventArgs(_socket));
            }
            catch (Exception ex)
            {
                ChannelFailure(this, ex);
            }
            return bytesSent;
        }

        /// <summary>
        ///     Send a new message to a connected socket.
        /// </summary>
        /// <param name="message">An array of type byte that contains the data to be sent.</param>
        /// <param name="size">The number of bytes to send.</param>
        /// <param name="offset">The position in the data buffer at which to begin sending data.</param>
        /// <param name="socketFlags">A bitwise combination of the SocketFlags values.</param>
        /// <param name="endPoint">An EndPoint that represents the remote device. </param>
        public int SendTo(byte[] message, int size, int offset, SocketFlags socketFlags, EndPoint endPoint)
        {
            int bytesSent = 0;
            try
            {
                bytesSent = _socket.SendTo(message, size, offset, socketFlags, endPoint);
                _messageSent(this, new SocketEventArgs(_socket));
            }
            catch (Exception ex)
            {
                ChannelFailure(this, ex);
            }
            return bytesSent;
        }

        /// <summary>
        ///     Send a new message to a connected socket.
        /// </summary>
        /// <param name="message">A message type Socket Buffer that contains the data to be sent.</param>
        public void Send(SocketBuffer message)
        {
            Stream sendMessage = new MemoryStream(message.Buffer);
            Send(sendMessage);
        }

        /// <summary>
        ///     Send a new message to a connected socket.
        /// </summary>
        /// <param name="message">A message type Stream that contains the data to be sent.</param>
        public int Send(Stream message)
        {
            byte[] sendBuffer = new byte[1460];
            int bytesSent = 0;
            try
            {
                if (message.Length > 0)
                {
                    int sentBytes = 0;
                    message.Position = 0;
                    while ((sentBytes = message.Read(sendBuffer, 0, sendBuffer.Length)) > 0)
                    {
                        bytesSent += _socket.Send(sendBuffer, sentBytes, SocketFlags.None);
                    }
                    _socket.Close();
                    _connected = false;
                    _messageSent(this, new SocketEventArgs(_socket));
                }
            }
            catch (Exception ex)
            {
                ChannelFailure(this, ex);
            }
            return bytesSent;
        }

        /// <summary>
        ///      Send a new message to a connected socket.
        /// </summary>
        /// <param name="message">An array of type byte that contains the data to be sent.</param>
        public int Send(byte[] message)
        {
            int bytesSent = 0;
            try
            {
                bytesSent = _socket.Send(message);
                _messageSent(this, new SocketEventArgs(_socket));
            }
            catch (Exception ex)
            {
                ChannelFailure(this, ex);
            }
            return bytesSent;
        }
        
        /// <summary>
        ///     Send a new message to a connected socket.
        /// </summary>
        /// <param name="message">An array of type byte that contains the data to be sent.</param>
        /// <param name="socketFlags">A bitwise combination of the SocketFlags values.</param>
        public int Send(byte[] message, SocketFlags socketFlags)
        {
            int bytesSent = 0;
            try
            {
                bytesSent = _socket.Send(message, socketFlags);
                _messageSent(this, new SocketEventArgs(_socket));
            }
            catch (Exception ex)
            {
                ChannelFailure(this, ex);
            }
            return bytesSent;
        }

        /// <summary>
        ///     Send a new message to a connected socket.
        /// </summary>
        /// <param name="message">An array of type byte that contains the data to be sent.</param>
        /// <param name="size">The number of bytes to send.</param>
        /// <param name="socketFlags">A bitwise combination of the SocketFlags values.</param>
        public int Send(byte[] message, int size, SocketFlags socketFlags)
        {
            int bytesSent = 0;
            try
            {
                bytesSent = _socket.Send(message, size, socketFlags);
                _messageSent(this, new SocketEventArgs(_socket));
            }
            catch (Exception ex)
            {
                ChannelFailure(this, ex);
            }
            return bytesSent;
        }

        /// <summary>
        ///     Send a new message to a connected socket.
        /// </summary>
        /// <param name="message">An array of type byte that contains the data to be sent.</param>
        /// <param name="offSet">The position in the data buffer at which to begin sending data.</param>
        /// <param name="size">The number of bytes to send.</param>
        /// <param name="socketFlags">A bitwise combination of the SocketFlags values.</param>
        public int Send(byte[] message, int offSet, int size, SocketFlags socketFlags)
        {
            int bytesSent = 0;
            try
            {
                bytesSent = _socket.Send(message, offSet, size, socketFlags);
                _socket.Close();
                _connected = false;
                _messageSent(this, new SocketEventArgs(_socket));
            }
            catch (Exception ex)
            {
                ChannelFailure(this, ex);
            }
            return bytesSent;
        }

        /// <summary>
        ///     Signal the channel to close.
        /// </summary>
        public void Close()
        {
            _socket.Close();
            _connected = false;
        }

        /// <summary>
        /// Detected a disconnect
        /// </summary>
        /// <param name="socketError">Socket failure reason.</param>
        protected void HandleDisconnect(SocketError socketError)
        {
            try
            {
                _socket.Close();
                _connected = false;
                _clientDisconnected(this, new SocketException(socketError));
            }
            catch (Exception ex)
            {
                ChannelError(this, ex);
            }
        }

        /// <summary>
        /// Detected a disconnect
        /// </summary>
        /// <param name="socketError">Socket failure reason.</param>
        /// <param name="exception">Why it was disconnected.</param>
        private void HandleDisconnect(SocketError socketError, Exception exception)
        {
            try
            {
                _socket.Close();
                _connected = false;
                _clientDisconnected(this, exception);
            }
            catch (Exception ex)
            {
                ChannelError(this, ex);
            }
        }

        #endregion Methods
        
        #region Events

        /// <summary>
        ///     An internal error occured
        /// </summary>
        public event ErrorHandler ChannelError = delegate { };

        #endregion Events

    }
}