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
        public Exception Failure { get; protected set; }

        /// <summary>
        /// Indicates that there was a failure during authentication.
        /// </summary>
        /// <param name="failure">The failure exception.</param>
        public static AuthenticateResult Fail(Exception failure)
        {
            return new AuthenticateResult() 
            { 
                Succeeded = false, 
                Failure = failure 
            };
        }
    }
}