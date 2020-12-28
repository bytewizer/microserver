using System;
using System.Collections;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Represents the <see cref="HttpRequest"/> and <see cref="HttpResponse"/> headers.
    /// </summary>
    public interface IHeaderDictionary : ICollection, IEnumerable
    {
        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="key"></param>
        string this[string key] { get; set; }

        /// <summary>
        /// Strongly typed access to the Content-Length header. 
        /// </summary>
        long ContentLength { get; set; }

        /// <summary>
        /// Strongly typed access to the If-Modified-Since header.
        /// </summary>
        DateTime IfModifiedSince { get; set; }
    }
}