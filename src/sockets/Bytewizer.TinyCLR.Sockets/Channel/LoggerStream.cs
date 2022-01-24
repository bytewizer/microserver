using System;
using System.IO;
using System.Text;
using System.Diagnostics;

#if NanoCLR
using Bytewizer.NanoCLR.Logging;

namespace Bytewizer.NanoCLR.Sockets.Channel
#else
using Bytewizer.TinyCLR.Logging;

namespace Bytewizer.TinyCLR.Sockets.Channel
#endif
{
    public class LoggerStream : Stream
    {       
        private readonly Stream _stream;
        private readonly ILogger _logger;

        private readonly EventId _readEvent = new EventId(1000, "Stream Read");
        private readonly EventId _writeEvent = new EventId(1100, "Stream Write");


        /// <summary>
        /// Implements a <see cref="Stream"/> that operates on a <see cref="Stream"/> object.
        /// </summary>
        public LoggerStream(Stream stream)
            : this(NullLogger.Instance, stream)
        {

        }

        /// <summary>
        /// Implements a <see cref="Stream"/> that operates on a <see cref="Stream"/> object.
        /// </summary>
        public LoggerStream(ILogger logger, Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            _stream = stream;
            _logger = logger;
        }

        /// <summary>
        /// Gets a value indicating whether the current stream supports reading.
        /// </summary>
        public override bool CanRead 
        {
            get { return _stream.CanRead; }
        }

        /// <summary>
        /// Gets a value indicating whether the current stream supports seeking.
        /// </summary>
        public override bool CanSeek
        {
            get { return _stream.CanSeek; }
        }

        /// <summary>
        /// Gets a value indicating whether the current stream supports writing.
        /// </summary>
        public override bool CanWrite
        {
            get { return _stream.CanWrite; }
        }

        /// <summary>
        /// Gets the length in bytes of the stream.
        /// </summary>
        public override long Length => _stream.Length;

        /// <summary>
        /// Gets or sets the position within the resource object.
        /// </summary>
        public override long Position 
        { 
            get => _stream.Position;
            set { _stream.Position = value; }
        }

        /// <summary>
        /// Clears all buffers for this resource manager and causes any buffered data to be written to the underlying device.
        /// </summary>
        public override void Flush() => _stream.Flush();


        /// <summary>
        /// Reads a sequence of bytes from the current resource object and advances the position within the stream by the number of bytes read.
        /// </summary>
        /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between offset and (offset + count - 1) replaced by the bytes read from the current source.</param>
        /// <param name="offset">The zero-based byte offset in buffer at which to begin storing the data read from the current stream.</param>
        /// <param name="count">The maximum number of bytes to be read from the resource manager.</param>
        /// <returns>The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.</returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            var read = _stream.Read(buffer, offset, count);

            Debug.WriteLine($"Stream Trace Read: {Encoding.UTF8.GetString(buffer, offset, count)}");

            _logger.Log(
               LogLevel.Trace,
               _readEvent,
               Encoding.UTF8.GetString(buffer, offset, count),
               null
               );

            return read;
        }

        /// <summary>
        /// Sets the position within the resource object.
        /// </summary>
        /// <param name="offset">A byte offset relative to the <b>origin</b> parameter.</param>
        /// <param name="origin">A value of type <see cref="SeekOrigin"/> indicating the reference point used to obtain the new position.</param>
        public override long Seek(long offset, SeekOrigin origin)
        {
            return _stream.Seek(offset,origin);
        }

        /// <summary>
        /// Sets the length of the current resource object. 
        /// </summary>
        /// <param name="value">The desired length of the current stream in bytes.</param>
        public override void SetLength(long value)
        {
            SetLength(value);
        }

        /// <summary>
        /// Writes a sequence of bytes to the resource object and advances the current position within this stream by the number of bytes written.
        /// </summary>
        /// <param name="buffer">An array of bytes. This method copies count bytes from buffer to the current stream.</param>
        /// <param name="offset">The zero-based byte offset in buffer at which to begin copying bytes to the current stream.</param>
        /// <param name="count">The number of bytes to be written to the current stream.</param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            Debug.WriteLine($"Stream Trace Write: {Encoding.UTF8.GetString(buffer, offset, count)}");

            _logger.Log(
              LogLevel.Trace,
              _writeEvent,
              Encoding.UTF8.GetString(buffer, offset, count),
              null
              );

            _stream.Write(buffer, offset, count);
        }
    }
}