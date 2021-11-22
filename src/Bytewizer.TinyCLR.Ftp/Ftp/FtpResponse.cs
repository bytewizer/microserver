using System;

namespace Bytewizer.TinyCLR.Ftp
{
    /// <summary>
    /// Represents the outgoing side of an individual FTP request.
    /// </summary>
    public class FtpResponse 
    {
        /// <summary>
        /// Gets the response message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets the response code.
        /// </summary>
        public override string ToString()
        {
            return Message.Replace("\r\n", string.Empty);
        }

        /// <summary>
        /// Clears the <see cref="FtpResponse"/> headers, cookies and body.
        /// </summary>
        public void Clear()
        {
            Message = default;
        }
    } 
}