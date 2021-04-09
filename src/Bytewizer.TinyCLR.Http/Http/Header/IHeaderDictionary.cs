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
        /// <param name="key">The key whose value to get or set.</param>
        string this[string key] { get; set; }

        /// <summary>
        /// Strongly typed access to the Content-Length header. 
        /// </summary>
        long ContentLength { get; set; }

        /// <summary>
        /// Strongly typed access to the If-Modified-Since header.
        /// </summary>
        DateTime IfModifiedSince { get; set; }

        /// <summary>
        /// Adds the specified element to the collection. If the specified header does not exist, the <see cref="Add"/> method inserts
        /// a new header into the list of header name/value pairs. If the specified header is already present, value is added to the
        /// comma-separated list of values associated with the header.
        /// </summary>
        /// <param name="key">The key to use as the key of the element to add.</param>
        /// <param name="value">The value of the header to add.</param>
        void Add(string key, string value);
    }
}