using Bytewizer.TinyCLR.Identity;

namespace Bytewizer.TinyCLR.Ftp
{
    /// <summary>
    /// Options for selecting authentication scheme and account services.
    /// </summary>
    public class AuthenticationOptions
    {
        /// <summary>
        /// Configuration for the <see cref="AuthenticationMiddleware"/>.
        /// </summary>
        public AuthenticationOptions()
        {
            IdentityProvider = new IdentityProvider();
        }

        /// <summary>
        /// Account service for the authentication scheme.
        /// </summary>
        public IIdentityProvider IdentityProvider { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether login is allowed by anonymous users.
        /// </summary>
        public bool AllowAnonymous { get; set; } = false;
    }
}