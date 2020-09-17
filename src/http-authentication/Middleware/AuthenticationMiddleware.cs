using System;
using System.Diagnostics;
using Bytewizer.TinyCLR.Http.Authenticator;
using Bytewizer.TinyCLR.Sockets;

namespace Bytewizer.TinyCLR.Http
{
    public class AuthenticationMiddleware : Middleware
    {
        private readonly AuthenticationOptions _options;

        public AuthenticationMiddleware(AuthenticationOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            _options = options;

            if (_options.AccountService == null)
            {
                throw new ArgumentException("Missing option AccountService implementation.");
            }
        }

        protected override void Invoke(HttpContext context, RequestDelegate next)
        {
            if (_options.AuthenticationScheme != AuthenticationSchemes.Anonymous)
            {

                if (_options.AuthenticationScheme == AuthenticationSchemes.None)
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return;
                }

                if (ValidateHeader(context, out string auth, out string schema))
                {
                    CreateChallenge(context, _options);
                }
                else if (ValidateSchema(schema))
                {
                    Debug.WriteLine($"The request schema '{schema}' is not supported");
                }
                else
                {
                    switch (_options.AuthenticationScheme)
                    {
                        case AuthenticationSchemes.Basic:
                            //BasicAuthentication(context);
                            break;
                        case AuthenticationSchemes.Digest:
                            //DigestAuthentication(context);
                            break;
                    }
                }

                // If we get here we can set authentication user
                context.User = "user";
            }

            next(context);
        }

        private static bool ValidateHeader(HttpContext context, out string auth, out string scheme)
        {
            var authHeader = context.Request.Headers.Authorization;

            auth = null;
            scheme = null;
            if (string.IsNullOrEmpty(authHeader))
            {
                return true;
            }

            var authorization = authHeader.Split(new[] { ' ' }, 2);
            if (authorization.Length != 2)
            {
                return true;
            }

            scheme = authorization[0];
            auth = authorization[1];
;           return false;
        }

        private bool ValidateSchema(string schema)
        {
            if ((_options.AuthenticationScheme == AuthenticationSchemes.Basic && schema == "Basic")
                || (_options.AuthenticationScheme == AuthenticationSchemes.Digest && schema == "Digest"))
            {
                return false;
            }

            return true;
        }

        private static void CreateChallenge(HttpContext context, AuthenticationOptions options)
        {
            switch (options.AuthenticationScheme)
            {
                case AuthenticationSchemes.Basic:
                    context.Response.Headers.WWWAuthenticate = 
                        BasicChallenge.ToResponse(options.Realm);        
                    break;
                case AuthenticationSchemes.Digest:
                    context.Response.Headers.WWWAuthenticate =
                        DigestChallenge.ToResponse(options.Realm);
                    break;
            }

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        }
    }
}