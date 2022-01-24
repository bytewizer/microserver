using System;
using System.IO;
using System.Resources;

namespace Bytewizer.TinyCLR.Http
{
    public class ResourceStream : Stream
    {
        private long _position;
        
        private readonly long _length;
        private readonly short _resourceId;
        private readonly ResourceManager _resourceManager;

        /// <summary>
        /// Implements a <see cref="Stream"/> that operates on a <see cref="ResourceManager"/> object.
        /// </summary>
        public ResourceStream(ResourceManager resourceManager, short resourceId)
        {
            _resourceId = resourceId;
            _resourceManager = resourceManager;
            _length = GetResourceObjectSize(2048);
        }

        private int GetResourceObjectSize(int chunkSize)
        {
            // TODO:  This is a hack.  How can i get the size of a resource without the data?
            // In order to send the content disposition headers you need to know the size of the body before sending it.

            int position = 0;
            while (true)
            {
                var buffer = (byte[])_resourceManager.GetObject(_resourceId, position, chunkSize);
                position += buffer.Length;

                if (buffer.Length < chunkSize)
                {
                    break;
                }
            };

            GC.Collect();
            GC.WaitForPendingFinalizers();

            return position;
        }

        /// <summary>
        /// Gets a value indicating whether the current stream supports reading.
        /// </summary>
        public override bool CanRead 
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether the current stream supports seeking.
        /// </summary>
        public override bool CanSeek
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating whether the current stream supports writing.
        /// </summary>
        public override bool CanWrite
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the length in bytes of the stream.
        /// </summary>
        public override long Length => _length;

        /// <summary>
        /// Gets or sets the position within the resource object.
        /// </summary>
        public override long Position 
        { 
            get => _position;
            set { _position = value; }
        }

        /// <summary>
        /// Clears all buffers for this resource manager and causes any buffered data to be written to the underlying device.
        /// This method is not supported and always throws a <see cref="NotSupportedException"/> execption.
        /// </summary>
        public override void Flush()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Reads a sequence of bytes from the current resource object and advances the position within the stream by the number of bytes read.
        /// </summary>
        /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between offset and (offset + count - 1) replaced by the bytes read from the current source.</param>
        /// <param name="offset">The zero-based byte offset in buffer at which to begin storing the data read from the current stream.</param>
        /// <param name="count">The maximum number of bytes to be read from the resource manager.</param>
        /// <returns>The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.</returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            long maxRead = _length - _position;
            if (maxRead <= 0) 
            {
                return 0;
            }

            var readBuffer = (byte[])_resourceManager.GetObject(_resourceId, (int)_position + offset, (count <= maxRead) ? count : (int)maxRead);
            _position += readBuffer.Length;
            
            Array.Copy(readBuffer, buffer, readBuffer.Length);

            return readBuffer.Length;
        }

        /// <summary>
        /// Sets the position within the resource object.
        /// This method is not supported and always throws a <see cref="NotSupportedException"/> execption.
        /// </summary>
        /// <param name="offset">A byte offset relative to the <b>origin</b> parameter.</param>
        /// <param name="origin">A value of type <see cref="SeekOrigin"/> indicating the reference point used to obtain the new position.</param>
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Sets the length of the current resource object. 
        /// This method is not supported and always throws a <see cref="NotSupportedException"/> execption.
        /// </summary>
        /// <param name="value">The desired length of the current stream in bytes.</param>
        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Writes a sequence of bytes to the resource object and advances the current position within this stream by the number of bytes written.
        /// This method is not supported and always throws a <see cref="NotSupportedException"/> execption.
        /// </summary>
        /// <param name="buffer">An array of bytes. This method copies count bytes from buffer to the current stream.</param>
        /// <param name="offset">The zero-based byte offset in buffer at which to begin copying bytes to the current stream.</param>
        /// <param name="count">The number of bytes to be written to the current stream.</param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }
    }
}