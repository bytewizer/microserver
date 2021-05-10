using System;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Represents configuration limits of server specific features.
    /// </summary>
    public class HttpServerLimits
    {
        private long _maxRequestBufferSize = 8 * 1024;
        private long _maxResponseBufferSize = 8 * 1024;
        private long _maxRequestBodySize = 1024 * 1024;

        private readonly string _minBufferMessage = "Buffer size must be greater then 1024";


        /// <summary>
        /// Gets or sets the maximum size of the request buffer. Defaults to 8,192 bytes (8 KB).
        /// </summary>
        public long MaxRequestBufferSize
        {
            get => _maxRequestBufferSize;
            set
            {
                if (value < 1024)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), _minBufferMessage);
                }
                _maxRequestBufferSize = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum size of the response buffer. Defaults to 8,192 bytes (8 KB).
        /// </summary>
        public long MaxResponseBufferSize
        {
            get => _maxResponseBufferSize;
            set
            {
                if (value < 1024)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), _minBufferMessage);
                }
                _maxResponseBufferSize = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum size of the response body. Defaults to 1,048,576 bytes (1 MB).
        /// </summary>
        public long MaxRequestBodyBufferSize
        {
            get => _maxRequestBodySize;
            set
            {
                if (value < 1024)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), _minBufferMessage);
                }
                _maxRequestBodySize = value;
            }
        }
    }
}