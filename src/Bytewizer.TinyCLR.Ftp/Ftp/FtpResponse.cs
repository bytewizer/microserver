namespace Bytewizer.TinyCLR.Ftp
{
    /// <summary>
    /// Represents the outgoing side of an individual FTP request.
    /// </summary>
    public class FtpResponse 
    {
        /// <summary>
        /// Initializes an instance of the <see cref="FtpResponse" /> class.
        /// </summary>
        public FtpResponse()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FtpResponse"/> class.
        /// </summary>
        /// <param name="code">The response code.</param>
        /// <param name="message">The response message.</param>
        public FtpResponse(int code, string message)
        {
            Code = code;
            Message = message;
        }

        /// <inheritdoc />
        public int Code { get; private set; }

        /// <summary>
        /// Gets the response message.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Gets the response code.
        /// </summary>
        public override string ToString()
        {
            return $"{Code:D3} {Message}".TrimEnd();
        }

        /// <summary>
        /// Clears the <see cref="FtpResponse"/> headers, cookies and body.
        /// </summary>
        public void Clear()
        {
            Code = default;
            Message = default;
        }
    } 
}