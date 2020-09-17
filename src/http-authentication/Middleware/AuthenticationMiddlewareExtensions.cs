using System;

using Bytewizer.TinyCLR.Sockets;

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
        /// <param name="app">The <see cref="ServerOptions"/> instance this method extends.</param>
        /// <param name="options">The <see cref="AuthenticationOptions"/> used to configure the middleware.</param>
        public static void UseAuthentication(this ServerOptions app, AuthenticationOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            app.UseMiddleware(new AuthenticationMiddleware(options));
        }
    }
}