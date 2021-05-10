namespace Bytewizer.TinyCLR.Ftp
{
    /// <summary>
    /// Represents the incoming side of an individual FTP request.
    /// </summary>
    public class FtpRequest
    {
        /// <summary>
        /// Initializes an instance of the <see cref="FtpRequest" /> class.
        /// </summary>
        public FtpRequest()
        {
            Command = new FtpCommand();
        }

        /// <summary>
        /// Gets the FTP command to be executed.
        /// </summary>
        public FtpCommand Command { get; set; }

        /// <summary>
        /// Gets or sets the working directory path.
        /// </summary>
        public string Path { get; set; } = "A:\\";

        /// <summary>
        /// Gets a value indicating whether the request has been authenticated.
        /// </summary>
        public bool IsAuthenticated { get; internal set; }

        /// <summary>
        /// Clears the <see cref="FtpResponse"/> headers, cookies and body.
        /// </summary>
        public void Clear()
        {
            Command.Clear();
        }
    }
}