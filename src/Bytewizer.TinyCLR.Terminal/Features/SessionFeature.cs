using Bytewizer.TinyCLR.Identity;

namespace Bytewizer.TinyCLR.Terminal.Features
{
    /// <summary>
    /// A feature interface for this session. Use <see cref="TerminalContext.Features"/>
    /// to access an instance associated with the current request.
    /// </summary>
    public class SessionFeature 
    {
        /// <summary>
        /// Gets or sets security information for the current request.
        /// </summary>
        public IIdentityProvider IdentityProvider { get; set; }

        /// <summary>
        /// Gets or sets a login user name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets a value indicating whether the request has been authenticated.
        /// </summary>
        public bool Authenticated { get; set; }
    }
}