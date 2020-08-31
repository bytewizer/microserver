using System;

namespace Bytewizer.TinyCLR.Http.Authenticator
{
    public class Authentication : IAuthenticator
    {
        private readonly IAccountService _userService;

        public Authentication(IAccountService userService)
        {
            if (userService == null)
                throw new ArgumentNullException(nameof(userService));

            _userService = userService;
        }

        public void CreateChallenge(HttpContext context, AuthenticationOptions options)
        {
            context.Response.Headers.WWWAuthenticate =
                DigestChallenge.ToChallengeString(options.Realm);

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        }

        public string Authenticate(HttpContext context, AuthenticationOptions options)
        {
            var authHeader = context.Request.Headers.Authorization;
            var authorization = DigestAuthentication.Parse(authHeader);

            var user = _userService.Lookup(authorization.UserName, context.Request.Host);
            if (user == null)
            {
                return null;
            }

            if (authorization.Response == "username")
            {
                return user.Username;
            }

            return null;
        }
    }
}
