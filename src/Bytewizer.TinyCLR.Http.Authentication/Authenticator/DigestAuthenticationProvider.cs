using System;
using System.Text;
using System.Threading;
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
        private readonly Random _random = new Random();
        private readonly char[] _splitParts = new char[] { '=' };
        private readonly char[] _trimSymbols = new char[] { ' ', '\"' };

        //private static Timer _timer;
        //static readonly Hashtable _nonces = new Hashtable();

        /// <summary>
        /// Initializes a new instance of the <see cref="DigestAuthenticationProvider"/> class.
        /// </summary>
        public DigestAuthenticationProvider()
        {
            Qop = "auth";
            Realm = AuthHelper.DefaultRealm;
            Algorithm = "MD5";
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
        /// A flag, indicating that the previous request from the client was rejected because the nonce value was stale.
        /// </summary>
        public string Stale { get; internal set; }

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
            //lock (_nonces)
            //{
            //    if (_timer == null)
            //        _timer = new Timer(ManageNonces, null, 15000, 15000);
            //}

            if (!AuthHelper.ValidateHeader(context, out string auth, out string scheme))
            {
                return new AuthenticateResult() { Succeeded = false };
            }
            else if (Scheme != scheme)
            {
                return AuthenticateResult.Fail(
                    new InvalidOperationException($"The request schema '{scheme}' is not supported"));
            }
            else
            {
                var parameters = ParseAuthorizationHeader(auth, out string response, out string nonce, out string username);

                //if (ValidateNonce(nonce))
                //{
                //    return AuthenticateResult.Fail(
                //        new InvalidOperationException("Invalid digest authentication header failed to find nonce."));
                //}

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

                return AuthenticateResult.Fail(new InvalidOperationException());
            }
        }

        /// <inheritdoc/>
        public void Challenge(HttpContext context)
        {
            var sb = new StringBuilder(128);
            var nonce = CreateNonce();

            if (Domain != null)
            {
                sb.Append($"Digest realm=\"{Realm}\", domain=\"{Domain}\", nonce=\"{nonce}\"");
            }
            else
            {
                sb.Append($"Digest realm=\"{Realm}\", nonce=\"{nonce}\"");
            }

            if (Opaque != null)
                sb.Append($", opaque=\"{Opaque}\"");

            if (Stale != null)
                sb.Append($", stale={Stale}");

            if (Algorithm != null)
                sb.Append($", algorithm={Algorithm}");

            if (Qop != null)
                sb.Append($", qop=\"{Qop}\"");

            sb.Append(", charset=\"UTF-8\"");

            context.Response.Headers[HeaderNames.WWWAuthenticate] = sb.ToString();
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        }

        /// <inheritdoc/>
        public void Forbid(HttpContext context)
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

        private string CreateNonce()
        {
            var bytes = new byte[16];
            _random.NextBytes(bytes);

            string nonce = string.Empty;
            for (int i = 0; i < 16; i++)
            {
                nonce += $"{bytes[i]:x02}";
            }

            return nonce;
        }

        //private bool ValidateNonce(string nonce)
        //{
        //    lock (_nonces)
        //    {
        //        if (_nonces.Contains(nonce))
        //        {
        //            if ((DateTime)_nonces[nonce] < DateTime.Now)
        //            {
        //                _nonces.Remove(nonce);
        //                return false;
        //            }

        //            return true;
        //        }
        //    }

        //    return false;
        //}

        //private void ManageNonces(object state)
        //{
        //    lock (_nonces)
        //    {
        //        foreach (DictionaryEntry pair in _nonces)
        //        {
        //            if ((DateTime)pair.Value >= DateTime.Now)
        //                continue;

        //            _nonces.Remove(pair.Key);
        //            return;
        //        }
        //    }
        //}

    }
}