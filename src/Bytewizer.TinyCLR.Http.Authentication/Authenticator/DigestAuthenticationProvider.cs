using System;
using System.Text;
using System.Collections;

using Bytewizer.TinyCLR.Http.Features;

using GHIElectronics.TinyCLR.Cryptography;

namespace Bytewizer.TinyCLR.Http.Authenticator
{
    /// <summary>
    /// The Digest Access Authentication scheme implemented as simple access authentication.
    /// </summary>
    public class DigestAuthenticationProvider : IAuthenticationProvider
    {
        private readonly MD5 _md5 = MD5.Create();
        private readonly char[] _splitParts = new char[] { '=' };
        private readonly char[] _trimSymbols = new char[] { ' ', '\"' };
        private readonly DateTime _epoch = new DateTime(2017, 1, 1, 0, 0, 0);

        /// <summary>
        /// Initializes a new instance of the <see cref="DigestAuthenticationProvider"/> class.
        /// </summary>
        public DigestAuthenticationProvider()
        {
            Qop = "auth";  // none, auth, auth-int
            Realm = AuthHelper.DefaultRealm;
            Algorithm = "MD5"; // none, MD5, MD5-sess
            StaleTimeOut = 300;
        }

        /// <inheritdoc/>
        public string Scheme
        {
            get { return "Digest"; }
        }

        /// <summary>
        /// To be displayed to users so they know which username and password to use.
        /// </summary>
        public string Realm { get; set; }

        /// <summary>
        /// A quoted, space-separated list of URIs that define the protection space.
        /// </summary>
        public string Domain { get; internal set; }

        /// <summary>
        /// In seconds
        /// </summary>
        public double StaleTimeOut { get; set; }

        /// <summary>
        /// A string of data, specified by the server, which should be returned by the client unchanged in the Authorization
        /// header of subsequent requests with URIs in the same protection space.
        /// </summary>
        public string Opaque { get; internal set; }

        /// <summary>
        /// A string indicating a pair of algorithms used to produce the digest and a checksum.
        /// </summary>
        public string Algorithm { get; internal set; }

        /// <summary>
        /// If present, it is a quoted string of one or more tokens indicating the "quality of protection" values supported by the server.
        /// </summary>
        public string Qop { get; internal set; }

        /// <inheritdoc/>
        public AuthenticateResult Authenticate(HttpContext context, AuthenticationOptions options)
        {
            if (!AuthHelper.ValidateHeader(context, out string auth, out string scheme))
            {
                return AuthenticateResult.Fail(
                    new InvalidOperationException());
            }
            else if (Scheme != scheme)
            {
                return AuthenticateResult.Fail(
                    new InvalidOperationException($"The request schema '{scheme}' is not supported"));
            }
            else
            {
                var parameters = ParseAuthorizationHeader(auth, out string response, out string nonce, out string username);
                if (parameters != null)
                {
                    if (!ValidateNonce(nonce, context.Connection.RemoteIpAddress.ToString(), out bool isStale))
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        return AuthenticateResult.Fail(
                            new InvalidOperationException("Invalid digest authentication header failed to find nonce."));
                    }
                    else if (isStale)
                    {
                        Challenge(context, true);

                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return AuthenticateResult.Fail(
                            new InvalidOperationException());
                    }

                    var user = options.AccountService.GetUser(username);
                    if (user != null)
                    {
                        if (response == DigestResponse(parameters, context.Request.Method, user.HA1))
                        {
                            var authenticationFeature = new HttpAuthenticationFeature
                            {
                                User = user,
                            };
                            context.Features.Set(typeof(IHttpAuthenticationFeature), authenticationFeature);
                            return new AuthenticateResult();
                        }
                        else
                        {
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            return AuthenticateResult.Fail(new InvalidOperationException());
                        }
                    }
                }

                return AuthenticateResult.Fail(new InvalidOperationException());
            }
        }

        /// <inheritdoc/>
        public void Challenge(HttpContext context)
        {
            Challenge(context, false);
        }

        private void Challenge(HttpContext context, bool isStale)
        {
            var stale = isStale ? "true" : null;
            var challenge = new StringBuilder(128);

            var nonce = CreateNonce(context.Connection.RemoteIpAddress.ToString());
            if (Domain != null)
            {
                challenge.Append($"Digest realm=\"{Realm}\", domain=\"{Domain}\", nonce=\"{nonce}\"");
            }
            else
            {
                challenge.Append($"Digest realm=\"{Realm}\", nonce=\"{nonce}\"");
            }

            if (Opaque != null)
                challenge.Append($", opaque=\"{Opaque}\"");

            if (stale != null)
                challenge.Append($", stale={stale}");

            if (Algorithm != null)
                challenge.Append($", algorithm={Algorithm}");

            if (Qop != null)
                challenge.Append($", qop=\"{Qop}\"");

            challenge.Append(", charset=\"UTF-8\"");

            context.Response.Headers[HeaderNames.WWWAuthenticate] = challenge.ToString();
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        }

        /// <inheritdoc/>
        public void Unauthorized(HttpContext context)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        }

        private string DigestResponse(Hashtable parameters, string method, string ha1)
        {
            var nonce = (string)parameters["nonce"];
            var uri = (string)parameters["uri"];
            var algorithm = (string)parameters["algorithm"];
            var qop = (string)parameters["qop"];
            var clientNonce = (string)parameters["cnonce"];
            var nonceCount = (string)parameters["nc"];
            var entity = (string)parameters["entity"];

            var a1 = algorithm != null && algorithm.ToLower() == "md5-sess"
                     ? $"{ha1}:{nonce}:{clientNonce}"
                     : ha1;

            var a2 = qop != null && qop.ToLower() == "auth-int"
                     ? $"{method}:{uri}:{CreateHash(entity)}"
                     : $"{method}:{uri}";

            var data = qop != null
                       ? $"{nonce}:{nonceCount}:{clientNonce}:{qop}:{CreateHash(a2)}"
                       : $"{nonce}:{CreateHash(a2)}";

            return CreateHash($"{a1}:{data}");
        }

        private Hashtable ParseAuthorizationHeader(string auth, out string response, out string nonce, out string username)
        {
            Hashtable parameters = new Hashtable();

            response = string.Empty;
            nonce = string.Empty;
            username = string.Empty;

            try
            {
                string[] items = auth.Split(',');
                foreach (string item in items)
                {
                    string[] parts = item.Split(_splitParts, 2);
                    if (parts.Length == 2)
                    {
                        parameters.Add(parts[0].Trim(_trimSymbols), parts[1].Trim(_trimSymbols));
                    }
                }

                if (!parameters.Contains("response") || !parameters.Contains("algorithm"))
                {
                    return null;
                }

                response = parameters.Contains("response")
                    ? (string)parameters["response"] : string.Empty;

                username = parameters.Contains("username")
                    ? (string)parameters["username"] : string.Empty;

                nonce = parameters.Contains("nonce")
                    ? (string)parameters["nonce"] : string.Empty;
            }
            catch
            {
                return null;
            }

            return parameters;
        }

        private string CreateHash(string value)
        {
            var encodedBytes = Encoding.UTF8.GetBytes(value);
            var hashBytes = _md5.ComputeHash(encodedBytes);

            var sb = new StringBuilder(64);
            foreach (var bytes in hashBytes)
            {
                sb.Append($"{bytes:x02}");
            }

            return sb.ToString();
        }

        private string CreateNonce(string ipAddress)
        {
            var timeStamp = (DateTime.UtcNow - _epoch).TotalSeconds;
            var privateHash = CreateHash($"{timeStamp}:{ipAddress}");

            return Convert.ToBase64String(Encoding.UTF8.GetBytes($"{timeStamp}:{privateHash}"));
        }

        private bool ValidateNonce(string nonce, string ipAddress, out bool isStale)
        {
            isStale = true;
            string decoded = Encoding.UTF8.GetString(Convert.FromBase64String(nonce));

            int pos = decoded.IndexOf(':');
            if (pos == -1)
            {
                return false;
            }

            var timeStamp = decoded.Substring(0, pos);
            if (double.TryParse(timeStamp, out double nonceTimeStamp))
            {
                var dateTime = _epoch.AddSeconds(nonceTimeStamp);
                isStale = dateTime.AddSeconds(StaleTimeOut) < DateTime.UtcNow;
            };

            var privateHash = decoded.Substring(pos + 1, decoded.Length - pos - 1);
            if (CreateHash($"{timeStamp}:{ipAddress}") == privateHash)
            {
                return true;
            }

            return false;
        }
    }
}