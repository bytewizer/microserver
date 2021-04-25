﻿using System;
using System.Collections;
using System.Text;
using System.Threading;

namespace Bytewizer.TinyCLR.IO.Compression
{
    /// <summary>
    /// A bunch of constants used in the Zlib interface.
    /// </summary>
    public static class ZlibConstants
    {
        /// <summary>
        /// The maximum number of window bits for the Deflate algorithm.
        /// </summary>
        public const int WindowBitsMax = 15; // 32K LZ77 window

        /// <summary>
        /// The default number of window bits for the Deflate algorithm.
        /// </summary>
        public const int WindowBitsDefault = WindowBitsMax;

        /// <summary>
        /// Indicates everything is A-OK
        /// </summary>
        public const int Z_OK = 0;

        /// <summary>
        /// Indicates that the last operation reached the end of the stream.
        /// </summary>
        public const int Z_STREAM_END = 1;

        /// <summary>
        /// The operation ended in need of a dictionary. 
        /// </summary>
        public const int Z_NEED_DICT = 2;

        /// <summary>
        /// There was an error with the stream - not enough data, not open and readable, etc.
        /// </summary>
        public const int Z_STREAM_ERROR = -2;

        /// <summary>
        /// There was an error with the data - not enough data, bad data, etc.
        /// </summary>
        public const int Z_DATA_ERROR = -3;

        /// <summary>
        /// There was an error with memory allocation.
        /// </summary>
        public const int Z_MEM_ERROR = -4;

        /// <summary>
        /// There was an error with the working buffer.
        /// </summary>
        public const int Z_BUF_ERROR = -5;

        /// <summary>
        /// The size of the working buffer used in the ZlibCodec class. Defaults to 8192 bytes.
        /// </summary>
        public const int WorkingBufferSizeDefault = 8192;

        /// <summary>
        /// The minimum size of the working buffer used in the ZlibCodec class.  Currently it is 128 bytes.
        /// </summary>
        public const int WorkingBufferSizeMin = 1024;
    }
}
