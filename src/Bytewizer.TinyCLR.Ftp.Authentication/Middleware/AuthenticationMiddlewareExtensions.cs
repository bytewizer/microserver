using System;

using Bytewizer.TinyCLR.Identity;
using Bytewizer.TinyCLR.Pipeline.Builder;

namespace Bytewizer.TinyCLR.Ftp
{
    /// <summary>
    /// Extension methods for the <see cref="AuthenticationMiddleware"/>.
    /// </summary>
    public static class AuthenticationMiddlewareExtensions
    {
        /// <summary>
        /// Enable authentication capabilities.
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder"/> instance this method extends.</param>
        /// <param name="idenityProvider">The <see cref="IIdentityProvider"/> used to configure the middleware.</param>
        public static IApplicationBuilder UseAuthentication(this IApplicationBuilder builder, IIdentityProvider idenityProvider)
        {
            var options = new AuthenticationOptions()
            {
                IdentityProvider = idenityProvider
            };

            return builder.UseAuthentication(options);
        }

        /// <summary>
        /// Enable authentication capabilities.
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder"/> instance this method extends.</param>
        /// <param name="options">The <see cref="AuthenticationOptions"/> used to configure the middleware.</param>
        public static IApplicationBuilder UseAuthentication(this IApplicationBuilder builder, AuthenticationOptions options)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return builder.Use(new AuthenticationMiddleware(options));
        }
    }
}