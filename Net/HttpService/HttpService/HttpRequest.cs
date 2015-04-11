using System.Security.Cryptography.X509Certificates;

using MicroServer.Net.Http.Messages;

namespace MicroServer.Net.Http
{
    /// <summary>
    /// A HTTP request where the included body have been parsed
    /// </summary>
    public class HttpRequest : HttpRequestBase, IHttpMessage
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpMethod">Method like <c>POST</c>.</param>
        /// <param name="pathAndQuery">Absolute path and query string (if one exist)</param>
        /// <param name="httpVersion">HTTP version like <c>HTTP/1.1</c></param>
        public HttpRequest(string httpMethod, string pathAndQuery, string httpVersion) : base(httpMethod, pathAndQuery, httpVersion)
        {
            Form = new ParameterCollection();
            Files = new HttpFileCollection();
            Cookies = new HttpCookieCollection();
        }

        /// <summary>
        /// Create a response for this request.
        /// </summary>
        /// <returns>Response</returns>
        public override IHttpResponse CreateResponse()
        {
            var response = new HttpResponse(200, "OK", HttpVersion);
            return response;
        }
    }
}
