using Bytewizer.TinyCLR.Identity;

namespace Bytewizer.TinyCLR.Telnet.Features
{
    /// <summary>
    /// A feature interface for this session. Use <see cref="TelnetContext.Features"/>
    /// to access an instance associated with the current request.
    /// </summary>
    public class SessionFeature 
    {
        /// <summary>
        /// Gets or sets security information for the current request.
        /// </summary>
        public IIdentityProvider IdentityProvider { get; set; }

        /// <summary>
        /// Gets or sets the selected <see cref="Endpoint"/> for the current request.
        /// </summary>
        public Endpoint Endpoint { get; set; }

        /// <summary>
        /// Gets or sets a login user name.
        /// </summary>
        public string UserName { get; set; }

    }
}