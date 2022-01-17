using System;
using System.IO;
using System.Text;

using GHIElectronics.TinyCLR.Devices.UsbClient;
using static GHIElectronics.TinyCLR.Devices.UsbClient.Cdc;

namespace Bytewizer.TinyCLR.Terminal.Channel
{
    /// <summary>
    /// Represents a socket channel between two end points.
    /// </summary>
    public class ConsoleChannel
    {
        private bool _cleared = true;

        /// <summary>
        /// Gets communications device class (CDC) for the connected client.
        /// </summary>
        public Cdc Client { get; internal set; }

        /// <summary>
        /// Gets or sets a byte array that can be used to share data within the scope of this channel.
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// Gets a <see cref="Stream"/> object representing the contents of the channel.
        /// </summary>
        public CdcStream InputStream { get; internal set; }

        /// <summary>
        /// Gets a <see cref="Stream"/> object representing the contents of the channel.
        /// </summary>
        public CdcStream OutputStream { get; internal set; }

        /// <summary>
        /// Assign a client to this channel.
        /// </summary>
        /// <param name="client">The connected client for channel.</param>
        public void Assign(Cdc client)
        {
            Assign(
                client,
                client.Stream,
                client.Stream
                );
        }

        /// <summary>
        /// Assign a client to this channel.
        /// </summary>
        /// <param name="client">The connected client for channel.</param>
        /// <param name="inputStream">The input contents of the socket channel.</param>
        /// <param name="outputStream">The output contents of the socket channel.</param>
        public void Assign(Cdc client, CdcStream inputStream, CdcStream outputStream)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            Client = client;
            InputStream = inputStream;
            OutputStream = outputStream;

            _cleared = false;
        }

        /// <summary>
        /// Determine whether the channel is cleared.
        /// </summary>
        public bool Cleared { get { return _cleared; } }

        /// <summary>
        /// Clears the connected client.
        /// </summary>
        public void Clear()
        {
            _cleared = true;
        }

        /// <summary>
        /// Reads a sequence of bytes from the connected socket channel and advances
        /// the position within the stream by the number of bytes read.
        /// </summary>
        /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array.</param>
        public int Read(byte[] buffer)
        {
            return Read(buffer, 0, buffer.Length);
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
            return Write($"{text}{Environment.NewLine}");
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
                OutputStream.Write(buffer);
                OutputStream.Flush();

                bytesSent = buffer.Length;
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
        protected virtual void OnChannelError(ConsoleChannel channel, Exception execption)
        {
            ChannelError(this, execption);
        }

        /// <summary>
        /// An event that is raised when an interanl channel error occured.
        /// </summary>
        public event ConsoleErrorHandler ChannelError = delegate { };
    }
}