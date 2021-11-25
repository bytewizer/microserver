using Bytewizer.TinyCLR.Identity;

namespace Bytewizer.TinyCLR.Ftp
{
    /// <summary>
    /// Options for selecting authentication scheme and account services.
    /// </summary>
    public class AuthenticationOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether login is allowed by anonymous users.
        /// </summary>
        public bool AllowAnonymous { get; set; } = false;

        /// <summary>
        /// Account service for the specified authentication scheme.
        /// </summary>
        public IIdentityProvider IdentityProvider { get; set; }
    }
}