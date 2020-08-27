using System;
using System.Text;
using System.Diagnostics;

using GHIElectronics.TinyCLR.Cryptography;

namespace Bytewizer.TinyCLR.Http.Authentication
{
    public class BasicAuthentication : IAuthenticator
    {
        private readonly string _realm;
        private readonly IAccountService _userService;

        public BasicAuthentication(IAccountService userService, string realm)
        {
            if (userService == null)
                throw new ArgumentNullException(nameof(userService));

            if (realm == null)
                throw new ArgumentNullException(nameof(realm));

            _userService = userService;
            _realm = realm;
        }

        public string AuthenticationScheme
        {
            get { return "Basic"; }
        }

        public void CreateChallenge(HttpContext context)
        {
            context.Response.Headers.WWWAuthenticate = $"{AuthenticationScheme} realm=\"{_realm}\"";
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        }

        public string Authenticate(HttpContext context)
        {
            string authHeader = context.Request.Headers.Authorization;

            string encodedCredentials = authHeader.Substring($"{AuthenticationScheme} ".Length).Trim();
            string credentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));

            int seperatorIndex = credentials.IndexOf(':');
            if (seperatorIndex == -1)
            {
                Debug.WriteLine($"Invalid basic authentication header failed to find seperator colon.");
                return null;
            }

            var username = credentials.Substring(0, seperatorIndex);
            var password = credentials.Substring(seperatorIndex + 1);

            var user = _userService.Lookup(username, context.Request.Host);
            if (user != null)
            {
                if (user.Password == null)
                {
                    string ha1 = Encode(context.Request.Host, username, password);

                    if (ha1 != user.HA1)
                    {
                        Debug.WriteLine($"Incorrect user name or password. User Name: {user.Username}");
                        return null;
                    }
                }
                else
                {
                    if (username != user.Username || password != user.Password)
                    {
                        Debug.WriteLine($"Incorrect user name or password. User Name: {user.Username}");
                        return null;
                    }
                }
            }

            return username;
        }

        private string Encode(string realm, string userName, string password)
        {
            var crypto = MD5.Create();
            var toHash = Encoding.UTF8.GetBytes($"{userName}:{realm}:{password}");
            var hash = BitConverter.ToString(crypto.ComputeHash(toHash));

            return hash;
        }
    }
}