using System;

namespace Bytewizer.TinyCLR.Http.Authenticator
{
    /// <summary>
    /// Contains the result of an authenticate call.
    /// </summary>
    public class AuthenticateResult
    {
        /// <summary>
        /// Indicates if authenticate was successful.
        /// </summary>
        public bool Succeeded { get; set; } = true;

        /// <summary>
        /// Holds failure information from the authentication.
        /// </summary>
        public string Failure { get; protected set; }

        /// <summary>
        /// Indicates that there was a failure during authentication.
        /// </summary>
        /// <param name="message">The failure message.</param>
        public static AuthenticateResult Fail(string message)
        {
            return new AuthenticateResult() 
            { 
                Succeeded = false, 
                Failure = message 
            };
        }
    }
}