using System.Net;
using MicroServer.Net.Http.Messages;
using System.IO;

namespace MicroServer.Net.Http
{
    /// <summary>
    /// Complete HTTP resposne.
    /// </summary>
    public class HttpResponse : HttpResponseBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpResponse"/> class.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <param name="reasonPhrase">The reason phrase.</param>
        /// <param name="httpVersion">The HTTP version.</param>
        internal HttpResponse(int statusCode, string reasonPhrase, string httpVersion) : base(statusCode, reasonPhrase, httpVersion)
        {
            Cookies = new HttpCookieCollection();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpResponse"/> class.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <param name="reasonPhrase">The reason phrase.</param>
        /// <param name="httpVersion">The HTTP version.</param>
        public HttpResponse(HttpStatusCode statusCode, string reasonPhrase, string httpVersion) : base(statusCode, reasonPhrase, httpVersion)
        {
            Cookies = new HttpCookieCollection();
        }

        /// <summary>
        /// Cookies to send to the server side
        /// </summary>
        public HttpCookieCollection Cookies { get; set; }
    }
}