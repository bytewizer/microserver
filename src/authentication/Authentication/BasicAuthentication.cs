using System;
using System.Text;
using System.Diagnostics;

using GHIElectronics.TinyCLR.Cryptography;

namespace Bytewizer.TinyCLR.Http.Authentication
{
    public class BasicAuthentication : IAuthenticator
    {
        private readonly string realm;
        private readonly IAccountService userService;

        public BasicAuthentication(IAccountService userService, string realm)
        {
            if (userService == null) 
                throw new ArgumentNullException(nameof(userService));
            
            if (realm == null) 
                throw new ArgumentNullException(nameof(realm));
            
            this.userService = userService;
            this.realm = realm;
        }

        public string Scheme
        {
            get { return "basic"; }
        }

        public string AuthenticationScheme
        {
            get { return "basic"; }
        }

        public void CreateChallenge(HttpContext context)
        {
            context.Response.Headers.Add("WWW-Authenticate", "Basic realm=\"" + realm + "\"");
            context.Response.StatusCode = 401;
        }

        public string Authenticate(HttpContext context)
        {
            string authHeader = context.Request.Headers["Authorization"];

            if (authHeader != null && authHeader.StartsWith("Basic"))
            {
                string encodedCredentials = authHeader.Substring("Basic ".Length).Trim();
                string credentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));

                int seperatorIndex = credentials.IndexOf(':');
                if (seperatorIndex == -1)
                {
                    Debug.WriteLine($"Invalid basic authentication header failed to find seperator colon.");
                    return null;
                }

                var username = credentials.Substring(0, seperatorIndex);
                var password = credentials.Substring(seperatorIndex + 1);

                var user = userService.Lookup(username, context.Request.Host);
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
            else
            {
                //Handle what happens if that isn't the case
                Debug.WriteLine("The authorization header is either empty or isn't Basic.");
                return null;
            }
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