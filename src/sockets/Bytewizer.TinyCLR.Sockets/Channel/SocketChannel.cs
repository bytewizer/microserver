using System;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;


#if NanoCLR
using System.Net.Security;

namespace Bytewizer.NanoCLR.Sockets.Channel
#else
using System.Security.Authentication;

namespace Bytewizer.TinyCLR.Sockets.Channel
#endif
{
    /// <summary>
    /// Represents a socket channel between two end points.
    /// </summary>
    public class SocketChannel
    {
        private bool _closed = true;
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
            Assign(
                socket, 
                new NetworkStream(socket, false), 
                new NetworkStream(socket, false)
                );
        }

        /// <summary>
        /// Assign a socket to this channel.
        /// </summary>
        /// <param name="socket">The connected socket for channel.</param>
        /// <param name="buffer">The connected socket buffer for channel.</param>
        /// <param name="endpoint">The remote endpoint of the connected socket. </param>
        public void Assign(Socket socket, byte[] buffer, EndPoint endpoint)
        {

#if NanoCLR
            Assign(socket, new MemoryStream(buffer), null);
#else
            Assign(socket, new MemoryStream(buffer, false), null);
#endif
            Connection.Assign(socket, endpoint);
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
            {
                throw new ArgumentNullException(nameof(socket));
            }

            var streamBuilder = new SslStreamBuilder(certificate, allowedProtocols);
            var stream = streamBuilder.Build(socket);
            
            Assign(socket, stream, stream);
        }

        /// <summary>
        /// Assign a socket to this channel.
        /// </summary>
        /// <param name="socket">The connected socket for channel.</param>
        /// <param name="inputStream">The input contents of the socket channel.</param>
        /// <param name="outputStream">The output contents of the socket channel.</param>
        public void Assign(Socket socket, Stream inputStream, Stream outputStream)
        {
            if (socket == null)
            {
                throw new ArgumentNullException(nameof(socket));
            }

            Client = socket;
            InputStream = inputStream;
            OutputStream = outputStream;
            Connection.Assign(socket);

            _cleared = false;
            _closed = false;
        }

        /// <summary>
        /// Determine whether the socket channel is connected.
        /// </summary>
        public bool Connected
        {
            get
            {
                if (!_closed)
                {
                    return !(Client.Poll(1000, SelectMode.SelectRead) && (Client.Available == 0));
                }
                return false;
            }
        }

        /// <summary>
        /// Closes the connection.
        /// </summary>
        public void Close()
        {
            _closed = true;
            Client.Close();
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
                Close();
            }

            InputStream?.Dispose();
            OutputStream?.Dispose();

            _cleared = true;
        }

        /// <summary>
        /// Reads a sequence of bytes from the connected socket channel and advances
        /// the position within the stream by the number of bytes read.
        /// </summary>
        /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array.</param>
        public int Read(byte[] buffer)
        {
            return Read(buffer, 0,buffer.Length);
        }

        /// <summary>
        /// Reads a sequence of bytes from the connected socket channel and advances
        /// the position within the stream by the number of bytes read.
        /// </summary>
        /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array.</param>
        /// <param name="offset">The zero-based byte offset in buffer at which to begin storing the data read from the current stream.</param>
        /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
        public int Read(byte[] buffer, int offset, int count)
        {
            int bytesRead = 0;
            try
            {
                bytesRead = InputStream.Read(buffer, offset, count);
            }
            catch (Exception ex)
            {
                OnChannelError(this, ex);
            }

            return bytesRead;
        }

        /// <summary>
        /// Reads a byte from the connected socket channel and advances the position within the stream
        /// by one byte, or return -1 if at the end of the stream.
        /// </summary>
        public int ReadByte()
        {
            int bytesRead = 0;
            try
            {
                bytesRead = InputStream.ReadByte();
            }
            catch (Exception ex)
            {
                OnChannelError(this, ex);
            }

            return bytesRead;
        }

        /// <summary>
        /// Writes a new response to the connected socket channel. UTF-8 encoding will be used.
        /// </summary>
        /// <param name="text">A <see cref="string"/> that contains data to be UTF8 encoded and sent.</param>
        public int WriteLine(string text)
        {
            return Write($"{text}\n\r");
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
        /// <param name="buffer">A <see cref="byte"/> array that contains data to be sent.</param>
        /// <returns>The number of bytes sent to the <see cref="Socket"/>.</returns>
        public int Write(byte[] buffer)
        {
            if (buffer.Length <= 0)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            int bytesSent = 0;
            try
            {
                if (Connected)
                {
                    OutputStream.Write(buffer);
                    OutputStream.Flush();

                    bytesSent = buffer.Length;
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
        /// <param name="buffer">An array of type byte that contains the data to be sent.</param>
        /// <param name="offSet">The position in the data buffer at which to begin sending data.</param>
        /// <param name="size">The number of bytes to send.</param>
        /// <param name="socketFlags">A bitwise combination of the SocketFlags values.</param>
        public int Send(byte[] buffer, int offSet, int size, SocketFlags socketFlags)
        {
            int bytesSent = 0;
            try
            {
                bytesSent = Client.Send(buffer, offSet, size, socketFlags);
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
        /// <param name="buffer">An array of type byte that contains the data to be sent.</param>
        /// <param name="offset">The position in the data buffer at which to begin sending data.</param>
        /// <param name="size">The number of bytes to send.</param>
        /// <param name="socketFlags">A bitwise combination of the SocketFlags values.</param>
        /// <param name="endPoint">An EndPoint that represents the remote device. </param>
        public int SendTo(byte[] buffer, int offset, int size, SocketFlags socketFlags, EndPoint endPoint)
        {
            int bytesSent = 0;
            try
            {
                bytesSent = Client.SendTo(buffer, offset, size, socketFlags, endPoint);
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