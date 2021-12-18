using Bytewizer.TinyCLR.Identity;

namespace Bytewizer.TinyCLR.Ftp.Features
{
    /// <summary>
    /// A feature interface for this session. Use <see cref="FtpContext.Features"/>
    /// to access an instance associated with the current request.
    /// </summary>
    public class SessionFeature 
    {
        /// <summary>
        /// Gets or sets security information for the current FTP request.
        /// </summary>
        public IIdentityProvider IdentityProvider { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether login is allowed by anonymous users.
        /// </summary>
        public bool AllowAnonymous { get; set; } = true;

        /// <summary>
        /// Gets or sets the from path for the <c>RNFR</c> command.
        /// </summary>
        public string FromPath { get; set; }

        /// <summary>
        /// Gets or sets the TLS block size for the <c>PBSZ</c> and <c>PROT</c> command.
        /// </summary>
        public int TlsBlockSize { get; set; }

        /// <summary>
        /// Gets or sets the TLS port for the <c>PORT</c> command.
        /// </summary>
        public string TlsProt { get; set; }

        /// <summary>
        /// Gets or sets the marking position for the <c>REST</c> command.
        /// </summary>
        public int RestPosition { get; set; }
    }
}