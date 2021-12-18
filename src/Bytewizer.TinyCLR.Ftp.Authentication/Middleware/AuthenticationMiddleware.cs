using System;

using Bytewizer.TinyCLR.Ftp.Features;

namespace Bytewizer.TinyCLR.Ftp
{
    /// <summary>
    /// A middleware that enables authentication capabilities.
    /// </summary>
    public class AuthenticationMiddleware : Middleware
    {
        private readonly AuthenticationOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationMiddleware"/> class.
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
        protected override void Invoke(FtpContext context, RequestDelegate next)
        {
            var feature = new SessionFeature()
            {
                IdentityProvider = _options.IdentityProvider,
                AllowAnonymous = _options.AllowAnonymous
            };
            
            context.Features.Set(typeof(SessionFeature), feature);
        }
    }
}