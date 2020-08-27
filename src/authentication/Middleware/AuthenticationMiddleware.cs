using Bytewizer.TinyCLR.Sockets;

using Bytewizer.TinyCLR.Http.Authentication;
using System.Diagnostics;

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
            if (!ValidateScheme(context, _authenticator))
            {
                Debug.WriteLine("The request contained an invalid authentication scheme");
                
                _authenticator.CreateChallenge(context);
                return;
            }
            else if (!ValidateAuthentication(context, _authenticator, out var user))
            {
                var authorization = context.Request.Headers.Authorization;
                Debug.WriteLine($"The request failed to authenticate authorization header {authorization}");

                _authenticator.CreateChallenge(context);
                return;
            }
            else
            {
                // If we get here we can set authentication user
                context.User = user;
            }

            next(context);
        }

        private static bool ValidateScheme(HttpContext context, IAuthenticator authenticator)
        {
            var authorization = context.Request.Headers.Authorization;

            if (string.IsNullOrEmpty(authorization) ||
                    !authorization.StartsWith(authenticator.AuthenticationScheme))
            {
                return false;
            }

            return true;
        }

        private static bool ValidateAuthentication(HttpContext context, IAuthenticator authenticator, out string user)
        {
            user = authenticator.Authenticate(context);
            if (user == null)
            {
                return false;
            }

            return true;
        }
    }
}