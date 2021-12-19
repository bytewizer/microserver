using System;

using Bytewizer.TinyCLR.Identity;
using Bytewizer.TinyCLR.Pipeline.Builder;

namespace Bytewizer.TinyCLR.Telnet
{
    /// <summary>
    /// Extension methods for the <see cref="AuthenticationMiddleware"/>.
    /// </summary>
    public static class AuthenticationMiddlewareExtensions
    {
        /// <summary>
        /// Enable authentication capabilities with a single user.
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder"/> instance this method extends.</param>
        /// <param name="username">The user to create.</param>
        /// <param name="password">The password for the user to hash and store.</param>
        public static IApplicationBuilder UseAuthentication(this IApplicationBuilder builder, string username, string password)
        {
            var options = new AuthenticationOptions()
            {
                IdentityProvider = new IdentityProvider()
            };

            var results = options.IdentityProvider.Create(new IdentityUser(username), password);
            if (results.Succeeded)
            {
                return builder.UseAuthentication(options);
            }
            else
            {
                throw new AggregateException("Failed to create username.", results.Errors);
            }
        }


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