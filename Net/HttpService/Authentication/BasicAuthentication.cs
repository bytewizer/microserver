using System;
using System.Net;
using System.Text;

using Microsoft.SPOT.Cryptography;

using MicroServer.Extensions;
using MicroServer.Net.Http;
using MicroServer.Net.Http.Exceptions;
using Microsoft.SPOT;
using MicroServer.Utilities;
using MicroServer.Logging;


namespace MicroServer.Net.Http.Authentication
{
    /// <summary>
    /// Basic authentication
    /// </summary>
    public class BasicAuthentication : IAuthenticator
    {
        private readonly string _realm;
        private readonly IAccountService _userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicAuthentication" /> class.
        /// </summary>
        /// <param name="userService">The user service.</param>
        /// <param name="realm">The realm.</param>
        /// <exception cref="System.ArgumentNullException">
        /// userService
        /// or
        /// realm
        /// </exception>
        public BasicAuthentication(IAccountService userService, string realm)
        {
            if (userService == null) throw new ArgumentNullException("userService");
            if (realm == null) throw new ArgumentNullException("realm");
            _userService = userService;
            _realm = realm;
        }

        /// <summary>
        /// Gets authenticator scheme
        /// </summary>
        /// <value></value>
        /// <example>
        /// digest
        /// </example>
        public string Scheme
        {
            get { return "basic"; }
        }

        #region IAuthenticator Members

        /// <summary>
        /// Create a WWW-Authenticate header
        /// </summary>
        public void CreateChallenge(IHttpRequest httpRequest, IHttpResponse response)
        {
            response.AddHeader("WWW-Authenticate", "Basic realm=\"" + _realm + "\"");
            response.StatusCode = 401;
        }

        /// <summary>
        /// Gets name of the authentication scheme
        /// </summary>
        /// <remarks>"BASIC", "DIGEST" etc.</remarks>
        public string AuthenticationScheme
        {
            get { return "basic"; }
        }

        /// <summary>
        /// Authorize a request.
        /// </summary>
        /// <param name="request">Request being authenticated</param>
        /// <returns>Authenticated user if successful; otherwise null.</returns>
        public string Authenticate(IHttpRequest request)
        {
            string authHeader = request.Headers["Authorization"];
            if (authHeader == null)
                return null;

            authHeader = authHeader.Replace("Basic ",string.Empty);

            string decoded = ByteUtility.GetString(Convert.FromBase64String(authHeader));

            var pos = decoded.IndexOf(':');
            if (pos == -1)
                Logger.WriteDebug(this, "Invalid basic authentication header, failed to find colon. Header: " + authHeader);

            var password = decoded.Substring(pos + 1, decoded.Length - pos - 1);
            var userName = decoded.Substring(0, pos);

            var user = _userService.Lookup(userName, request.Uri);
            if (user == null)
                return null;

            if (user.Password == null)
            {
                var ha1 = GetHa1(request.Uri.Host, userName, password);
                if (ha1 != user.HA1)
                {
                    Logger.WriteInfo(this, "Incorrect user name or password. User Name: " + user.Username);
                    return null;
                }
            }
            else
            {
                if (password != user.Password)
                {
                    Logger.WriteInfo(this, "Incorrect user name or password. User Name: " + user.Username);
                    return null;
                }
            }

            return user.Username;
        }

        /// <summary>
        /// Generate a HA1 hash
        /// </summary>
        /// <param name="realm">Realm that the user want to authenticate in</param>
        /// <param name="userName">User Name</param>
        /// <param name="password">Password</param>
        /// <returns></returns>
        public static string GetHa1(string realm, string userName, string password)
        {
            return GetMd5HashBinHex(String.Concat(userName, ":", realm, ":", password));
        }

        /// <summary>
        /// Gets the Md5 hash bin hex2.
        /// </summary>
        /// <param name="toBeHashed">To be hashed.</param>
        /// <returns></returns>
        public static string GetMd5HashBinHex(string toBeHashed)
        {
            MD5 hashAlgorithm = new MD5();
            hashAlgorithm.HashCore(Encoding.UTF8.GetBytes(toBeHashed), 0, toBeHashed.Length);
            byte[] buffer = hashAlgorithm.HashFinal();

            char[] output = new char[buffer.Length * 2];
            for (int i = 0; i < buffer.Length; i++)
            {
                output[(i << 1) + 0] = (buffer[i] >> 4) <= 9 ? (char)((buffer[i] >> 4) + '0') : (char)((buffer[i] >> 4) - 10 + 'A');
                output[(i << 1) + 1] = (buffer[i] & 15) <= 9 ? (char)((buffer[i] & 15) + '0') : (char)((buffer[i] & 15) - 10 + 'A');
            }

            return output.ToString();
        }

        #endregion
    }
}