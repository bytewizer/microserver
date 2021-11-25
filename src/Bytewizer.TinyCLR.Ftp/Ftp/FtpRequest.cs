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
            TransferMode = TransferMode.Stream;
            TransferType = TransferType.Image;
            StructureType = StructureType.File;
            ListFormat = ListFormat.Unix;
        }

        /// <summary>
        /// Gets the FTP command to be executed.
        /// </summary>
        public FtpCommand Command { get; internal set; }

        public DataMode DataMode { get; internal set; }

        public TransferMode TransferMode { get; internal set; }      

        public TransferType TransferType { get; internal set; }

        public StructureType StructureType { get; internal set; }

        public ListFormat ListFormat { get; internal set; }

        /// <summary>
        /// Gets or sets the user name for this user.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether the request has been authenticated.
        /// </summary>
        public bool IsAuthenticated { get; internal set; }

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
            TransferMode = TransferMode.Stream;
            TransferType = TransferType.Image;
            StructureType = StructureType.File;
            ListFormat = ListFormat.Unix;
            Name = string.Empty;
            IsAuthenticated = false;
        }
    }
}