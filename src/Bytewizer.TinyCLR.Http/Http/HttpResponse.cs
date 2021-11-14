using System;
using System.IO;

using Bytewizer.TinyCLR.Http.Header;

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
            Body = new MemoryStream();
        }

        /// <summary>
        /// Gets the <see cref="HttpContext"/> for this request.
        /// </summary>
        public HttpContext HttpContext { get; internal set; }

        /// <summary>
        /// Gets the response headers.
        /// </summary>
        public IHeaderDictionary Headers { get; }

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

        /// <summary>
        /// Clears the <see cref="HttpResponse"/> headers, cookies and body.
        /// </summary>
        public void Clear()
        {
            ((HeaderDictionary)Headers).Clear();
            Body = new MemoryStream();
            HttpContext = null;
            StatusCode = 0;
        }
    } 
}