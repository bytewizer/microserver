﻿using System;
using System.Text;

using Bytewizer.TinyCLR.Identity;
using Bytewizer.TinyCLR.Http.Features;

using GHIElectronics.TinyCLR.Cryptography;

namespace Bytewizer.TinyCLR.Http.Authenticator
{
    /// <summary>
    /// The Basic access authentication scheme. This scheme is not considered to be a secure method of user 
    /// authentication(unless used in conjunction with some external secure system such as SSL, as the user name and password are passed over the network as cleartext.
    /// </summary>
    public class BasicAuthenticationProvider : IAuthenticationProvider
    {
        private readonly MD5 _md5 = MD5.Create();

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicAuthenticationProvider"/> class.
        /// </summary>
        public BasicAuthenticationProvider()
        {
            Realm = AuthHelper.DefaultRealm;
        }

        /// <inheritdoc/>
        public string Scheme
        {
            get { return "Basic"; }
        }

        /// <summary>
        /// To be displayed to users so they know which username and password to use.
        /// </summary>
        public string Realm { get; set; }

        /// <inheritdoc/>
        public AuthenticateResult Authenticate(HttpContext context, AuthenticationOptions options)
        {
            if (!AuthHelper.ValidateHeader(context, out string auth, out string scheme))
            {
                Challenge(context);
                return AuthenticateResult.Fail($"Invalid authentication header '{auth}'");
            }
            else if (Scheme != scheme)
            {
                Challenge(context);
                return AuthenticateResult.Fail($"The request schema '{scheme}' is not supported");
            }
            else
            {
                string decoded = Encoding.UTF8.GetString(Convert.FromBase64String(auth));
                int pos = decoded.IndexOf(':');
                if (pos == -1)
                {
                    return AuthenticateResult.Fail($"The request authorization token '{decoded}' was invalid");
                }

                string username = decoded.Substring(0, pos);
                string password = decoded.Substring(pos + 1, decoded.Length - pos - 1);

                if (options.IdentityProvider.TryGetUser(username, out IIdentityUser user))
                {
                    if ((string)user.Metadata == CreateHA1Hash(username, Realm, password))
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
                        Challenge(context);
                        return AuthenticateResult.Fail("The request included an invalid username or password");
                    }
                }
            }

            return AuthenticateResult.Fail("The request authorization failed to include required or supported properties");
        }

        /// <inheritdoc/>
        public void Challenge(HttpContext context)
        {
            context.Response.Headers[HeaderNames.WWWAuthenticate] = $"{Scheme} realm=\"{Realm}\", charset=\"UTF-8\"";
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        }

        /// <inheritdoc/>
        public void Forbid(HttpContext context)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        }

        /// <summary>
        /// Method to pre-compute the "A1" MD5 hash.
        /// </summary>
        private string CreateHA1Hash(string username, string realm, string secret)
        {
            var encodedBytes = Encoding.UTF8.GetBytes($"{username}:{realm}:{secret}");
            var hashBytes = _md5.ComputeHash(encodedBytes);

            var sb = new StringBuilder(64);
            foreach (var bytes in hashBytes)
            {
                sb.Append($"{bytes:x02}");
            }

            return sb.ToString();
        }
    }
}
