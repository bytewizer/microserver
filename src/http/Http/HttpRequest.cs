﻿using System.IO;

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
        /// Gets or sets the request path base.
        /// </summary>
        public string PathBase { get; set; }

        /// <summary>
        /// Gets the collection of route values for this request.
        /// </summary>
        public RouteDictionary RouteValues { get; set; }
    }
}