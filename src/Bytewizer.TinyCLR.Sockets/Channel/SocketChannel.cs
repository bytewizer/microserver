using System;
using System.IO;
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
        /// <summary>
        /// Gets socket for the connected end point.
        /// </summary>
        public Socket Socket { get; internal set; }

        /// <summary>
        /// Gets socket information for the connected end point.  
        /// </summary>
        public ConnectionInfo Connection { get; internal set; }

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
        /// <param name="certificate">The X.509 certificate for channel.</param>
        /// <param name="allowedProtocols">The possible versions of ssl protocols channel allows.</param>
        public void Assign(Socket socket, X509Certificate certificate, SslProtocols allowedProtocols)
        {
            if (socket == null)
                throw new ArgumentNullException(nameof(socket));

            var streamBuilder = new SslStreamBuilder(certificate, allowedProtocols);

            Socket = socket;
            InputStream = streamBuilder.Build(socket);
            Connection = ConnectionInfo.Set(socket);
        }

        /// <summary>
        /// Determine whether the socket channel is closed.
        /// </summary>
        /// <param name="timeoutMicroSeconds"></param>
        public bool IsClosed(int timeoutMicroSeconds = 1)
        {
            if (Socket == null)
                return true;

            return Socket.Poll(timeoutMicroSeconds, SelectMode.SelectRead) && Socket.Available < 1;
        }

        /// <summary>
        /// Clears the connected socket channel.
        /// </summary>
        public void Clear()
        {
            if (Socket != null)
            {
                Socket.Close();
                Socket = null;
            }

            if (Connection != null)
            {
                Connection = null;
            }

            if (InputStream != null)
            {
                InputStream.Close();
                InputStream = null;
            }

            if (OutputStream != null)
            {
                OutputStream.Close();
                OutputStream = null;
            }
        }

        /// <summary>
        /// Writes a new response to the connected socket channel.
        /// </summary>
        /// <param name="text">A <see cref="string"/> that contains data to be sent.</param>
        public void Write(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            var bytes = Encoding.UTF8.GetBytes(text);
            OutputStream?.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Writes a new response to the connected socket channel.
        /// </summary>
        /// <param name="bytes">A <see cref="byte"/> array that contains data to be sent.</param>
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
                bytesSent += bytes.Length;
            }
            catch (Exception)
            {
            }

            return bytesSent;
        }

        /// <summary>
        /// Writes a new response to the connected socket channel.
        /// </summary>
        /// <param name="stream">A <see cref="Stream"/> that contains data to be sent.</param>
        public int Write(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

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
            catch (Exception)
            {
            }

            return bytesSent;
        }
    }
}