using System.IO;

using Bytewizer.TinyCLR.Http.Header;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Represents the incoming side of an individual HTTP request.
    /// </summary>
    public class HttpRequest
    {
        public HttpRequest()
        {
            Headers = new HeaderDictionary();
            Query = new QueryCollection();
            Body = new MemoryStream();
            Path = string.Empty;
        }

        /// <summary>
        /// Gets the request headers.
        /// </summary>
        public HeaderDictionary Headers { get; set; }

        /// <summary>
        /// Gets the query value collection.
        /// </summary>
        public QueryCollection Query { get; set; }

        /// <summary>
        /// Gets or sets the request body Stream.
        /// </summary>
        public Stream Body { get; set; }

        /// <summary>
        /// Gets or sets the HTTP method.
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Gets or sets the request protocol (e.g. HTTP/1.1).
        /// </summary>
        public string Protocol { get; set; }

        /// <summary>
        /// Gets or sets the request path.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the value for the Content-Length response header.
        /// </summary>
        public long ContentLength
        {
            get { return Headers.ContentLength; }
            set { Headers.ContentLength = value; }
        }

        /// <summary>
        /// Gets or sets the value for the Content-Type response header.
        /// </summary>
        public string ContentType
        {
            get { return Headers.ContentType; }
            set { Headers.ContentType = value; }
        }

        /// <summary>
        /// Gets or sets the value for the Host response header.
        /// </summary>
        public string Host
        {
            get { return Headers.Host; }
            set { Headers.Host = value; }
        }

        /// <summary>
        /// Gets the collection of Cookies for this request.
        /// </summary>
        public ICookieCollection Cookies { get; set; }

        /// <summary>
        /// Gets the request body as a form for this request.
        /// </summary>
        public IFormCollection Form { get; set; }

        /// <summary>
        /// Gets the collection of route values for this request.
        /// </summary>
        public IRouteValueDictionary RouteValues { get; set; }
    }
}