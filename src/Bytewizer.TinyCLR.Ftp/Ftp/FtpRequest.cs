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
            DataMode = DataMode.None;
            DataType = DataType.Image;
            ListFormat = ListFormat.Unix;
        }

        /// <summary>
        /// Gets the FTP command to be executed.
        /// </summary>
        public FtpCommand Command { get; set; }

        public DataMode DataMode { get; set; }

        public DataType DataType { get; set; }

        public ListFormat ListFormat { get; set; }

        /// <summary>
        /// Gets a value indicating whether the request has been authenticated.
        /// </summary>
        public bool IsAuthenticated { get; internal set; }

        /// <summary>
        /// Clears the <see cref="FtpResponse"/> object.
        /// </summary>
        public void Clear()
        {
            Command.Clear();
            DataMode = DataMode.None;
            DataType = DataType.Image;
            ListFormat = ListFormat.Unix;
            IsAuthenticated = false;
        }
    }
}