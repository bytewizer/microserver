using Bytewizer.TinyCLR.Identity;

namespace Bytewizer.TinyCLR.Ftp.Features
{
    /// <summary>
    /// A feature interface for authentication. Use <see cref="FtpContext.Features"/>
    /// to access an instance associated with the current request.
    /// </summary>
    public interface IFtpAuthenticationFeature
    {
        /// <summary>
        /// Gets or sets a value indicating whether login is allowed by anonymous users.
        /// </summary>
        bool AllowAnonymous { get; }

        /// <summary>
        /// Gets or sets security information for the current FTP request.
        /// </summary>
        IIdentityProvider IdentityProvider { get; }
    }
}