using System;
using Bytewizer.TinyCLR.Pipeline;
using Bytewizer.TinyCLR.Terminal.Features;

namespace Bytewizer.TinyCLR.Terminal
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
        protected override void Invoke(ITerminalContext context, RequestDelegate next)
        {
            var endpointFeature = (SessionFeature)context.Features.Get(typeof(SessionFeature));
            endpointFeature.IdentityProvider = _options.IdentityProvider;

            next(context);
        }
    }
}