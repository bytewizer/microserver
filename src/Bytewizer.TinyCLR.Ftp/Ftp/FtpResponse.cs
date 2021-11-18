using System;

namespace Bytewizer.TinyCLR.Ftp
{
    /// <summary>
    /// Represents the outgoing side of an individual FTP request.
    /// </summary>
    public class FtpResponse 
    {
        private int _code = 0;

        /// <summary>
        /// Initializes an instance of the <see cref="FtpResponse" /> class.
        /// </summary>
        public FtpResponse()
        {
        }

        /// <inheritdoc />
        public int Code
        {
            get
            {
                return _code;
            }
            set
            {
                if (value < 100 || value > 999)
                {
                    throw new ArgumentException("Protocol violation no such status code.");
                }
                _code = value;
            }
        }

        /// <summary>
        /// Gets the response message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets the response code.
        /// </summary>
        public override string ToString()
        {
            return $"{Code:D3} {Message}".TrimEnd();
        }

        /// <summary>
        /// Clears the <see cref="FtpResponse"/> code and message.
        /// </summary>
        public void Clear()
        {
            _code = default;
            Message = default;
        }
    } 
}