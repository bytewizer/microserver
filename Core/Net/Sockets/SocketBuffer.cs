using System;

namespace MicroServer.Net.Sockets
{
    /// <summary>
    ///     Socket buffer used by the socket channel
    /// </summary>
    public class SocketBuffer
    {
        #region Public Properties

        /// <summary>
        ///     The data buffer to use with socket method.
        /// </summary>
        public byte[] Buffer { get; private set; }

        /// <summary>
        ///     The length of valid content.
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        ///     The number of bytes buffer has allocated.
        /// </summary>
        public int Capacity { get; private set; }

        /// <summary>
        ///     The position in the data buffer.
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        ///     The base position in the data buffer.
        /// </summary>
        public int BaseOffset { get; set; }

        /// <summary>
        ///     The total number of bytes transferred to or from socket buffer.
        /// </summary>
        public int BytesTransferred { get; set; }

        #endregion Public Properties

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SocketBuffer" /> class.
        /// </summary>
        public SocketBuffer()
        {
            Buffer = new byte[65535];
            Capacity = 65535;
            Offset = 0;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SocketBuffer" /> class.
        /// </summary>
        /// <param name="size">The number of bytes used to create the buffer .</param>
        public SocketBuffer(int size)
        {
            if (size == 0 || size > 65535)
                throw new ArgumentOutOfRangeException("size", "Size must be greater then 0 and less then 65536");

            Buffer = new byte[size];
            Capacity = size;
            Offset = 0;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        ///     Sets the data buffer to use with socket method.
        /// </summary>
        /// <param name="buffer">The data buffer to use with an asynchronous socket method.</param>
        public void SetBuffer(byte[] buffer)
        {
            if (buffer.Length > Capacity)
                throw new ArgumentOutOfRangeException("buffer", "buffer length must be less then or equal to the socket buffer capacity");

            Buffer = buffer;
            Length = buffer.Length;
            Offset = 0;
        }

        /// <summary>
        ///     Sets the data buffer to use with socket method.
        /// </summary>
        /// <param name="offset">The offset, in bytes, in the data buffer where the operation starts.</param>
        /// <param name="length">The maximum amount of data, in bytes, to send or receive in the buffer.</param>
        public void SetBuffer(int offset, int length)
        {
            Offset = offset;
            Length = length;
        }

        /// <summary>
        ///     Sets the data buffer to use with socket method.
        /// </summary>
        /// <param name="buffer">The data buffer to use with an asynchronous socket method.</param>
        /// <param name="offset">The offset, in bytes, in the data buffer where the operation starts.</param>
        /// <param name="length">The maximum amount of data, in bytes, to send or receive in the buffer.</param>
        public void SetBuffer(byte[] buffer, int offset, int length)
        {
            Buffer = buffer;
            Length = length;
            Offset = offset;
        }

        /// <summary>
        ///     Sets the data buffer to use with socket method.
        /// </summary>
        /// <param name="buffer">The data buffer to use with an asynchronous socket method.</param>
        /// <param name="offset">The offset, in bytes, in the data buffer where the operation starts.</param>
        /// <param name="length">The maximum amount of data, in bytes, to send or receive in the buffer.</param>
        /// <param name="capacity">The number of bytes buffer has allocated. </param>
        public void SetBuffer(byte[] buffer, int offset, int length, int capacity)
        {
            Buffer = buffer;
            Offset = offset;
            Length = length;
            Capacity = capacity;
        }

        /// <summary>
        /// Clears the data buffer.
        /// </summary>
        public void Clear()
        { 
            Buffer = new byte[Capacity];
            BytesTransferred = 0;
            Length = 0;
            Offset = 0;
            BaseOffset = 0;
        }

        #endregion Methods
    }
}