using Bytewizer.TinyCLR.Http.Authenticator;

namespace Bytewizer.TinyCLR.Http
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
            AuthenticationProvider = new DigestAuthenticationProvider();      
            AccountService = new DefaultAccountService();           
        }

        /// <summary>
        /// Authentication service for the specified authentication scheme.
        /// </summary>
        public IAuthenticationProvider AuthenticationProvider { get; set; }

        /// <summary>
        /// Account service for the specified authentication scheme.
        /// </summary>
        public IAccountService AccountService { get; set; }
    }
}