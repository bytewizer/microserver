using System.Text;

using GHIElectronics.TinyCLR.Cryptography;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Provides helper methods for authentications.
    /// </summary>
    public static class AuthHelper
    {
        /// <summary>
        /// The default realm used by authentication providers.
        /// </summary>
        public static string DefaultRealm = "TinyCLR";

        /// <summary>
        /// Validate the authorization header parts.
        /// </summary>
        /// <param name="context">The context for the request.</param>
        /// <param name="auth">The requested authentication parameters part.</param>
        /// <param name="scheme">The requested scheme part.</param>
        public static bool ValidateHeader(HttpContext context, out string auth, out string scheme)
        {
            auth = null;
            scheme = null;

            var authHeader = context.Request.Headers.Authorization;
            if (string.IsNullOrEmpty(authHeader))
            {
                return false;
            }

            var authorization = authHeader.Split(new[] { ' ' }, 2);
            if (authorization.Length != 2)
            {
                return false;
            }

            scheme = authorization[0];
            auth = authorization[1];

            return true;
        }

        /// <summary>
        /// Method to pre-compute the "A1" MD5 hash.
        /// </summary>
        public static string ComputeA1Hash(string username, string secret)
        {
            return ComputeA1Hash(username, DefaultRealm, secret);
        }

        /// <summary>
        /// Method to pre-compute the "A1" MD5 hash.
        /// </summary>
        public static string ComputeA1Hash(string username, string realm, string secret)
        {
            var encodedBytes = Encoding.UTF8.GetBytes($"{username}:{realm}:{secret}");

            using (var md5 = MD5.Create())
            {
                var hashBytes = md5.ComputeHash(encodedBytes);

                var sb = new StringBuilder(128);
                foreach (var bytes in hashBytes)
                {
                    sb.Append($"{bytes:x02}");
                }

                return sb.ToString();
            }
        }
    }
}