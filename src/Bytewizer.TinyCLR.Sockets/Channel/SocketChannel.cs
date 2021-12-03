using System;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace Bytewizer.TinyCLR.Sockets.Channel
{
    /// <summary>
    /// Represents a socket channel between two end points.
    /// </summary>
    public class SocketChannel
    {
        private bool _cleared = true;

        /// <summary>
        /// Gets socket for the connected endpoint.
        /// </summary>
        public Socket Client { get; internal set; }

        /// <summary>
        /// Gets socket information for the connected endpoint.  
        /// </summary>
        public ConnectionInfo Connection { get; internal set; } = new ConnectionInfo();

        /// <summary>
        /// Gets or sets a byte array that can be used to share data within the scope of this channel.
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// Gets a <see cref="Stream"/> object representing the contents of the socket channel.
        /// </summary>
        public Stream InputStream { get; internal set; }

        /// <summary>
        /// Gets a <see cref="Stream"/> object representing the contents of the socket channel.
        /// </summary>
        public Stream OutputStream { get; internal set; }

        /// <summary>
        /// Assign a socket to this channel.
        /// </summary>
        /// <param name="socket">The connected socket for channel.</param>
        public void Assign(Socket socket)
        {
            if (socket == null)
                throw new ArgumentNullException(nameof(socket));

            Client = socket;
            InputStream = new NetworkStream(socket, false);
            OutputStream = new NetworkStream(socket, false);
            Connection.Assign(socket);
            
            _cleared = false;
        }

        /// <summary>
        /// Assign a socket to this channel.
        /// </summary>
        /// <param name="socket">The connected socket for channel.</param>
        /// <param name="buffer">The connected socket buffer for channel.</param>
        /// <param name="endpoint">The remote endpoint of the connected socket. </param>
        public void Assign(Socket socket, byte [] buffer, EndPoint endpoint)
        {
            if (socket == null)
                throw new ArgumentNullException(nameof(socket));

            Client = socket;
            InputStream = new MemoryStream(buffer, false);
            OutputStream = null;
            Connection.Assign(socket, endpoint);

            _cleared = false;
        }

        /// <summary>
        /// Assign a socket to this channel.
        /// </summary>
        /// <param name="socket">The connected socket for channel.</param>
        /// <param name="certificate">The X.509 certificate for channel.</param>
        /// <param name="allowedProtocols">The possible versions of ssl protocols channel allows.</param>
        public void Assign(Socket socket, X509Certificate certificate, SslProtocols allowedProtocols)
        {
            if (socket == null)
                throw new ArgumentNullException(nameof(socket));

            var streamBuilder = new SslStreamBuilder(certificate, allowedProtocols);
            var stream = streamBuilder.Build(socket); 

            Client = socket;
            InputStream = stream;
            OutputStream = stream;
            Connection.Assign(socket);

            _cleared = false;
        }

        /// <summary>
        /// Determine whether the socket channel is connected.
        /// </summary>
        public bool Connected
        {
            get { return !(Client.Poll(1000, SelectMode.SelectRead) && (Client.Available == 0)); }
        }

        /// <summary>
        /// Determine whether the socket channel is cleared.
        /// </summary>
        public bool Cleared { get { return _cleared; } }

        /// <summary>
        /// Closes and clears the connected socket channel.
        /// </summary>
        public void Clear()
        {
            if (InputStream.GetType() == typeof(NetworkStream)) // TODO: This feels like a hack.  Better way?
            {
                Client?.Close();
            }
            
            InputStream?.Dispose();
            OutputStream?.Dispose();

            _cleared = true;
        }

        /// <summary>
        /// Writes a new response to the connected socket channel. UTF-8 encoding will be used.
        /// </summary>
        /// <param name="text">A <see cref="string"/> that contains data to be UTF8 encoded and sent.</param>
        public int Write(string text)
        {
            return Write(text, Encoding.UTF8);
        }

        /// <summary>
        /// Writes a new response to the connected socket channel.
        /// </summary>
        /// <param name="text">A <see cref="string"/> that contains data to be sent.</param>
        /// <param name="encoding">The encoding to use.</param>
        public int Write(string text, Encoding encoding)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            int bytesSent = 0;
            try
            {
                var bytes = encoding.GetBytes(text);
                bytesSent += Write(bytes);
            }
            catch (Exception ex)
            {
                OnChannelError(this, ex);
            }

            return bytesSent;
        }

        /// <summary>
        /// Writes a new response to the connected socket channel.
        /// </summary>
        /// <param name="bytes">A <see cref="byte"/> array that contains data to be sent.</param>
        /// <returns>The number of bytes sent to the <see cref="Socket"/>.</returns>
        public int Write(byte[] bytes)
        {
            if (bytes.Length <= 0)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            int bytesSent = 0;
            try
            {
                if (Connected)
                {
                    OutputStream?.Write(bytes);
                    OutputStream.Flush();

                    bytesSent = bytes.Length;
                }
            }
            catch (Exception ex)
            {
                OnChannelError(this, ex);
            }

            return bytesSent;
        }

        /// <summary>
        /// Writes a new response to the connected socket channel.
        /// </summary>
        /// <param name="stream">A <see cref="Stream"/> that contains data to be sent.</param>
        public long Write(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            long bytesSent = 0;
            try
            {
                if (stream.Length > 0 && Connected)
                {
                    stream?.CopyTo(OutputStream);
                    OutputStream.Flush();
                    bytesSent = stream.Length;
                }
            }
            catch (Exception ex)
            {
                
                OnChannelError(this, ex);
            }

            return bytesSent;

        }

        /// <summary>
        /// Send a new message to a connected channel client.
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
                bytesSent = Client.Send(message, offSet, size, socketFlags);
            }
            catch (Exception ex)
            {
                OnChannelError(this, ex);
            }
            return bytesSent;
        }

        /// <summary>
        /// Send a new message to a connected remote device.
        /// </summary>
        /// <param name="message">An array of type byte that contains the data to be sent.</param>
        /// <param name="offset">The position in the data buffer at which to begin sending data.</param>
        /// <param name="size">The number of bytes to send.</param>
        /// <param name="socketFlags">A bitwise combination of the SocketFlags values.</param>
        /// <param name="endPoint">An EndPoint that represents the remote device. </param>
        public int SendTo(byte[] message, int offset, int size, SocketFlags socketFlags, EndPoint endPoint)
        {
            int bytesSent = 0;
            try
            {
                bytesSent = Client.SendTo(message, offset, size, socketFlags, endPoint);
            }
            catch (Exception ex)
            {
                OnChannelError(this, ex);
            }
            return bytesSent;
        }

        /// <summary>
        /// An internal channel error occured.
        /// </summary>
        protected virtual void OnChannelError(SocketChannel channel, Exception execption)
        {
            ChannelError(this, execption);
        }

        /// <summary>
        /// An event that is raised when an interanl channel error occured.
        /// </summary>
        public event SocketErrorHandler ChannelError = delegate { };
    }
}