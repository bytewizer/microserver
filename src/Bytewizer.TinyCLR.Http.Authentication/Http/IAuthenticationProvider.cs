using Bytewizer.TinyCLR.Http.Authenticator;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Used to provide authentication.
    /// </summary>
    public interface IAuthenticationProvider
    {
        /// <summary>
        /// The authenticator scheme.
        /// </summary>
        string Scheme { get; }

        /// <summary>
        /// Authenticate for the specified authentication scheme.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/>.</param>
        /// <param name="options">The <see cref="AuthenticationOptions"/> used to configure the middleware.</param>
        AuthenticateResult Authenticate(HttpContext context, AuthenticationOptions options);

        /// <summary>
        /// Challenge the specified authentication scheme.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/>.</param>
        void Challenge(HttpContext context);

        /// <summary>
        /// Forbids the specified authentication scheme.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/>.</param>
        void Unauthorized(HttpContext context);
    }
}