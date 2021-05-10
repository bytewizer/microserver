using System;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

using Bytewizer.TinyCLR.Sockets.Client;
using Bytewizer.TinyCLR.Sockets.Handlers;

namespace Bytewizer.TinyCLR.Sockets.Channel
{
    /// <summary>
    /// Represents a socket channel between two end points.
    /// </summary>
    public class SocketChannel
    {
        /// <summary>
        /// Gets socket for the connected endpoint.
        /// </summary>
        public TcpClient Client { get; internal set; }

        /// <summary>
        /// Gets or sets a value that indicates whether a connection has been made.
        /// </summary>
        protected bool Active => Client.Active;

        /// <summary>
        /// Gets socket information for the connected endpoint.  
        /// </summary>
        public ConnectionInfo Connection { get; internal set; }

        /// <summary>
        /// Gets or sets a byte array that can be used to share data within the scope of this channel.
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// Gets a <see cref="NetworkStream"/> object representing the contents of the socket channel.
        /// </summary>
        public Stream InputStream { get; internal set; }

        /// <summary>
        /// Gets a <see cref="NetworkStream"/> object representing the contents of the socket channel.
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

            Client = new TcpClient(socket);
            InputStream = new NetworkStream(socket);
            OutputStream = new NetworkStream(socket);
            Connection = ConnectionInfo.Set(socket);
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

            Client = new TcpClient(socket);
            InputStream = new MemoryStream(buffer);
            OutputStream = new MemoryStream();
            Connection = ConnectionInfo.Set(socket, endpoint);
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

            Client = new TcpClient(socket);
            InputStream = streamBuilder.Build(socket);
            OutputStream = new NetworkStream(socket);
            Connection = ConnectionInfo.Set(socket);
        }

        /// <summary>
        /// Determine whether the socket channel is cleared.
        /// </summary>
        public bool IsCleared()
        {
            if (Connection == null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Closes and clears the connected socket channel.
        /// </summary>
        public void Clear()
        {
            Connection = null;
            Client?.Dispose();
            InputStream?.Dispose();
            OutputStream?.Dispose();
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
                OutputStream?.Write(bytes, 0, bytes.Length);
                OutputStream.Flush();

                bytesSent = bytes.Length;
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
                if (stream.Length > 0)
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