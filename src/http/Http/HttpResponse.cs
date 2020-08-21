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
        public HttpResponse()
        {
            Headers = new HeaderDictionary
            {
                //Date = DateTime.UtcNow.ToString("R"),
                ContentType = "text/html; charset=UTF-8"
            };
            Body = new MemoryStream();
        }

        /// <summary>
        /// Gets the response headers.
        /// </summary>
        public HeaderDictionary Headers { get; set; }

        /// <summary>
        /// Gets or sets the response body <see cref="Stream"/>.
        /// </summary>
        public Stream Body { get; set; }

        /// <summary>
        /// Gets or sets the value for the Content-Length response header.
        /// </summary>
        public long ContentLength { get; set; }

        /// <summary>
        /// Gets or sets the value for the Content-Type response header.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the HTTP response code.
        /// </summary>
        public int StatusCode { get; set; }
    } 
}