using System.Diagnostics;

using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.Http.Authenticator;

namespace Bytewizer.TinyCLR.Http
{
    public class AuthenticationMiddleware : Middleware
    {
        private readonly AuthenticationOptions _options;
        private readonly IAuthenticator _authenticator;

        public AuthenticationMiddleware(AuthenticationOptions options)
        {
            _options = options;
            _authenticator = options.Authenticator;
        }

        protected override void Invoke(HttpContext context, RequestDelegate next)
        {
            var authorization = context.Request.Headers.Authorization;

            if (string.IsNullOrEmpty(authorization))
            {
                _authenticator.CreateChallenge(context, _options);
                return;
            }
            else
            {
                var user = _authenticator.Authenticate(context, _options);
                if (user == null)
                {
                    Debug.WriteLine($"The request failed to authenticate authorization header {authorization}");
                    return;
                }

                // If we get here we can set authentication user
                context.User = user;
            }

            next(context);
        }
    }
}