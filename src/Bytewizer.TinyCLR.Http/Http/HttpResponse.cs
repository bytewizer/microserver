using System;
using System.IO;

using Bytewizer.TinyCLR.Http.Header;
using Bytewizer.TinyCLR.Http.Cookies;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Represents the outgoing side of an individual HTTP request.
    /// </summary>
    public class HttpResponse 
    {
        /// <summary>
        /// Initializes an instance of the <see cref="HttpResponse" /> class.
        /// </summary>
        public HttpResponse()
        {
            Headers = new HeaderDictionary();
            Cookies = new CookieCollection();
            Body = new MemoryStream();
        }

        /// <summary>
        /// Gets the response headers.
        /// </summary>
        public IHeaderDictionary Headers { get; }

        /// <summary>
        /// Gets an object that can be used to manage cookies for this response.
        /// </summary>
        public IResponseCookies Cookies { get; }

        /// <summary>
        /// Gets or sets the response body <see cref="Stream"/>.
        /// </summary>
        public Stream Body { get; set; }

        /// <summary>
        /// Gets or sets the HTTP response code.
        /// </summary>
        public int StatusCode { get; set; }

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
            get { return Headers[HeaderNames.ContentType]; }
            set { Headers[HeaderNames.ContentType] = value; }
        }
    } 
}