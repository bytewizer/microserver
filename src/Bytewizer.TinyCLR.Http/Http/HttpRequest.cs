using System.IO;
using System.Text;

using Bytewizer.TinyCLR.Http.Query;
using Bytewizer.TinyCLR.Http.Header;
using Bytewizer.TinyCLR.Http.Cookies;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Represents the incoming side of an individual HTTP request.
    /// </summary>
    public class HttpRequest
    {
        /// <summary>
        /// Initializes an instance of the <see cref="HttpRequest" /> class.
        /// </summary>
        public HttpRequest()
        {
            Headers = new HeaderDictionary();
            Query = new QueryCollection();
            Cookies = new CookieCollection();
            Body = new MemoryStream();
        }

        /// <summary>
        /// Gets the request headers.
        /// </summary>
        public IHeaderDictionary Headers { get; set; }

        /// <summary>
        /// Gets the query value collection.
        /// </summary>
        public IQueryCollection Query { get; set; }

        /// <summary>
        /// Gets the collection of Cookies for this request.
        /// </summary>
        public ICookieCollection Cookies { get; set; }

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
        /// Gets or sets the base path for the request. The path base should not end with a trailing slash.
        /// </summary>
        /// <returns>The base path for the request.</returns>
        public string PathBase { get; set; }

        /// <summary>
        /// Gets or sets the value for the Content-Length response header.
        /// </summary>
        public long ContentLength
        {
            get { return ((HeaderDictionary)Headers).ContentLength; }
            set { ((HeaderDictionary)Headers).ContentLength = value; }
        }

        /// <summary>
        /// Gets or sets the value for the Content-Type response header.
        /// </summary>
        public string ContentType
        {
            //get { return ((HeaderDictionary)Headers).ContentType; }
            //set { ((HeaderDictionary)Headers).ContentType = value; }
            get { return Headers[HeaderNames.ContentType]; }
            set { Headers[HeaderNames.ContentType] = value; }
        }

        /// <summary>
        /// Gets or sets the value for the Host response header.
        /// </summary>
        public string Host
        {
            get { return Headers[HeaderNames.Host]; }
            set { Headers[HeaderNames.Host] = value; }
        }

        /// <summary>
        /// Get string from the current HTTP request
        /// </summary>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Request method: {Method}");
            sb.AppendLine($"Request URL: {Path}");
            sb.AppendLine($"Request protocol: {Protocol}");
            sb.AppendLine($"Request body: {Body.Length}");
            foreach (QueryValue item in Query)
            {
                sb.AppendLine($"Request parameters: {item.Key}: {item.Value}");
            }
            foreach (HeaderValue header in Headers)
            {
                sb.AppendLine($"{header.Key}: {header.Value}");
            }

            return sb.ToString();
        }
    }
}