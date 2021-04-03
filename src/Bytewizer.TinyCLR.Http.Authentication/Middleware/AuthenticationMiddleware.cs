using Bytewizer.TinyCLR.Http.Authenticator;
using System;
using System.Diagnostics;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// A middleware that enables authentication capabilities.
    /// </summary>
    public class AuthenticationMiddleware : Middleware
    {
        private readonly AuthenticationOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationMiddleware"/> class. Defaults to <see cref="DigestAuthenticationProvider"/> authentication provider.
        /// </summary>
        /// <param name="options">The <see cref="AuthenticationOptions"/> used to configure the middleware.</param>
        public AuthenticationMiddleware(AuthenticationOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _options = options;
        }

        /// <inheritdoc/>
        protected override void Invoke(HttpContext context, RequestDelegate next)
        {
            if (string.IsNullOrEmpty(context.Request.Headers[HeaderNames.Authorization]))
            {
                _options.AuthenticationProvider.Challenge(context);
                return;
            }
            else
            {
                var results = _options.AuthenticationProvider.Authenticate(context, _options);
                Debug.WriteLine(results.Failure);
                if (results.Succeeded)
                {
                    next(context);
                    return;
                }
                else
                {
                    return;
                }
            }

            //_options.AuthenticationProvider.Forbid(context);
        }
    }
}