using System;
using System.Net;

namespace MicroServer.Net.Http.Exceptions
{
    /// <summary>
    ///     The request was malformed.
    /// </summary>
    /// <remarks>
    ///     <para>Uses 400 as status code</para>
    /// </remarks>
    [Serializable]
    public class BadRequestException : HttpException
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="BadRequestException" /> class.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        public BadRequestException(string errorMessage)
            : base(HttpStatusCode.BadRequest, errorMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BadRequestException" /> class.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="inner">Inner exception.</param>
        public BadRequestException(string errorMessage, Exception inner)
            : base(HttpStatusCode.BadRequest, errorMessage, inner)
        {
        }
    }
}