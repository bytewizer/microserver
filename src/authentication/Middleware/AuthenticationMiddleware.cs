using Bytewizer.TinyCLR.Sockets;

using Bytewizer.TinyCLR.Http.Authentication;

namespace Bytewizer.TinyCLR.Http
{
    public class AuthenticationMiddleware : Middleware
    {
        private readonly IAuthenticator authenticator;

        public AuthenticationMiddleware(AuthenticationOptions options)
        {
            authenticator = options.Authenticator;
        }

        protected override void Invoke(HttpContext context, RequestDelegate next)
        {
            var user = authenticator.Authenticate(context);
            if (user == null)
            {
                authenticator.CreateChallenge(context);
                return;
            }

            context.User = user;

            next(context);
        }
    }
}