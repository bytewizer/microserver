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
            SecurityType = SecurityType.None;
            TransferMode = TransferMode.Stream;
            TransferType = TransferType.Image;
            StructureType = StructureType.File;
        }

        /// <summary>
        /// Gets the FTP command to be executed.
        /// </summary>
        public FtpCommand Command { get; internal set; }

        /// <summary>
        /// Gets the data mode for this request.
        /// </summary>
        public DataMode DataMode { get; internal set; }

        /// <summary>
        /// Gets the transport security type for this request.
        /// </summary>
        public SecurityType SecurityType { get; internal set; }

        /// <summary>
        /// Gets the transfer mode for this request.
        /// </summary>
        public TransferMode TransferMode { get; internal set; }

        /// <summary>
        /// Gets the file transfer type for this request.
        /// </summary>
        public TransferType TransferType { get; internal set; }

        /// <summary>
        /// Gets the file structure for this request.
        /// </summary>
        public StructureType StructureType { get; internal set; }

        /// <summary>
        /// Gets or sets the user name for this user.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether the request has been authenticated.
        /// </summary>
        public bool Authenticated { get; internal set; }

        /// <summary>
        /// Gets the command argument in upper case.
        /// </summary>
        public string Argument { get => Command?.Argument.ToUpper(); }

        /// <summary>
        /// Clears the <see cref="FtpResponse"/> object.
        /// </summary>
        public void Clear()
        {
            Command.Clear();

            DataMode = DataMode.None;
            SecurityType = SecurityType.None;
            TransferMode = TransferMode.Stream;
            TransferType = TransferType.Image;
            StructureType = StructureType.File;
            Name = string.Empty;
            Authenticated = false;
        }
    }
}