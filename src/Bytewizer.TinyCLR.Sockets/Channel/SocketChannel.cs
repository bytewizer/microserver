using System;
using System.IO;
using System.Net;
using System.Text;
using System.Collections;
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
        /// <summary>
        /// Gets socket for the connected endpoint.
        /// </summary>
        public Socket Socket { get; internal set; }

        /// <summary>
        /// Gets socket information for the connected endpoint.  
        /// </summary>
        public ConnectionInfo Connection { get; internal set; }

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

            Socket = socket;
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

            Socket = socket;
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

            Socket = socket;
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
            OutputStream?.Dispose();
            InputStream?.Dispose();
            Socket?.Close();
        }

        /// <summary>
        /// Writes a new response to the connected socket channel.
        /// </summary>
        /// <param name="text">A <see cref="string"/> that contains data to be UTF8 encoded and sent.</param>
        public int Write(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            int bytesSent = 0;
            try
            {
                var bytes = Encoding.UTF8.GetBytes(text);
                bytesSent += Write(bytes);
                //OutputStream?.Write(bytes, 0, bytes.Length);
                //bytesSent += bytes.Length;
            }
            catch
            {
                throw;  //TODO: Best way to handle?
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

            using (Stream output = new MemoryStream(bytes))
            {
                return Write(output);
            }

            //int bytesSent = 0;
            //try
            //{
            //    OutputStream?.Write(bytes, 0, bytes.Length);
            //    bytesSent += bytes.Length;
            //}
            //catch 
            //{
            //    throw;  //TODO: Best way to handle?
            //}

            //return bytesSent;
        }

        /// <summary>
        /// Writes a new response to the connected socket channel.
        /// </summary>
        /// <param name="stream">A <see cref="Stream"/> that contains data to be sent.</param>
        public int Write(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            byte[] sendBuffer = new byte[1460];
            int bytesSent = 0;
            try
            {
                if (stream.Length > 0)
                {
                    int sentBytes = 0;
                    stream.Position = 0;
                    while ((sentBytes = stream.Read(sendBuffer, 0, sendBuffer.Length)) > 0)
                    {
                        OutputStream?.Write(sendBuffer, 0, sentBytes);
                        bytesSent += sentBytes;
                    }
                }
            }
            catch
            {
                throw; //TODO: Best way to handle?
            }

            return bytesSent;
        }


        /// <summary>
        /// Send a new message to a connected socket.
        /// </summary>
        /// <param name="message">An array of type byte that contains the data to be sent.</param>
        public int Send(byte[] message)
        {
            int bytesSent;
            try
            {
                bytesSent = Socket.Send(message);
            }
            catch
            {
                throw; //TODO: Best way to handle?
            }
            return bytesSent;
        }

        /// <summary>
        /// Sends data to connected socket channel.
        /// </summary>
        /// <param name="buffer">An array of type <see cref="byte"/> that contains the data to be sent.</param>
        /// <param name="size">The number of bytes to send.</param>
        /// <param name="offset">The position in the data buffer at which to begin sending data.</param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="SocketFlags"/> values.</param>
        /// <returns>The number of bytes sent to the <see cref="Socket"/>.</returns>
        public int Send(byte[] buffer, int size, int offset, SocketFlags socketFlags)
        {
            int bytesSent;
            try
            {
                bytesSent = Socket.Send(buffer, size, offset, socketFlags);
            }
            catch
            {
                throw; //TODO: Best way to handle?
            }

            return bytesSent;
        }

        /// <summary>
        /// Sends data to a specific remote endpoint.
        /// </summary>
        /// <param name="buffer">An array of type <see cref="byte"/> that contains the data to be sent.</param>
        /// <param name="size">The number of bytes to send.</param>
        /// <param name="offset">The position in the data buffer at which to begin sending data.</param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="SocketFlags"/> values.</param>
        /// <param name="remoteEP">The <see cref="EndPoint"/> that represents the destination location for the data.</param>
        /// <returns>The number of bytes sent.</returns>
        public int SendTo(byte[] buffer, int size, int offset, SocketFlags socketFlags, EndPoint remoteEP)
        {
            int bytesSent;
            try
            {
                bytesSent = Socket.SendTo(buffer, size, offset, socketFlags, remoteEP);
            }
            catch
            {
                throw; //TODO: Best way to handle?
            }

            return bytesSent;
        }
    }
}