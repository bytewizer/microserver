namespace Bytewizer.TinyCLR.Ftp
{
    /// <summary>
    /// Defines FTP transfer modes.
    /// </summary>
    public enum TransferMode
    {
        /// <summary>
        /// Stream transfer mode.
        /// </summary>
        Stream,

        /// <summary>
        /// Block transfer mode.
        /// </summary>
        Block,

        /// <summary>
        /// Compress transfer mode.
        /// </summary>
        Compressed,

        /// <summary>
        /// Deflate transfer mode.
        /// </summary>
        Deflate
    }
}