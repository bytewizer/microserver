namespace Bytewizer.TinyCLR.Ftp
{
    /// <summary>
    /// Defines FTP transport layer security types.
    /// </summary>
    public enum SecurityType
    {
        /// <summary>
        /// No security type defined.
        /// </summary>
        None,

        /// <summary>
        /// Transport Layer Security (TLS) type.
        /// </summary>
        Tls,

        /// <summary>
        /// Secure Sockets Layer (SSL) type.
        /// </summary>
        Ssl,
    }
}