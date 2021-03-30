using System;

using Bytewizer.TinyCLR.Pipeline.Builder;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Extension methods for the <see cref="AuthenticationMiddleware"/>.
    /// </summary>
    public static class AuthenticationMiddlewareExtensions
    {

        /// <summary>
        /// Enable authentication capabilities.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> instance this method extends.</param>
        /// <param name="accountService">The <see cref="IAccountService"/> used to configure the middleware.</param>
        public static IApplicationBuilder UseAuthentication(this IApplicationBuilder app, IAccountService accountService)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (accountService == null)
            {
                throw new ArgumentNullException(nameof(accountService));
            }

            var options = new AuthenticationOptions()
            {
                AccountService = accountService
            };

            return app.UseMiddleware(typeof(AuthenticationMiddleware), options);
        }

        /// <summary>
        /// Enable authentication capabilities.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> instance this method extends.</param>
        /// <param name="options">The <see cref="AuthenticationOptions"/> used to configure the middleware.</param>
        public static IApplicationBuilder UseAuthentication(this IApplicationBuilder app, AuthenticationOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return app.UseMiddleware(typeof(AuthenticationMiddleware), options);
        }
    }
}