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

        /// <summary>
        /// Strongly typed access to the Connection header.
        /// </summary>
        string Connection { get; set; }

        /// <summary>
        /// Strongly typed access to the Upgrade header.
        /// </summary>
        string Upgrade { get; set; }

        /// <summary>
        /// Strongly typed access to the SecWebSocketAccept header.
        /// </summary>
        string SecWebSocketAccept { get; set; }

        /// <summary>
        /// Strongly typed access to the LastModified header.
        /// </summary>
        string LastModified { get; set; }

        /// <summary>
        /// Strongly typed access to the WWW-Authenticate header.
        /// </summary>
        string WWWAuthenticate { get; set; }

        /// <summary>
        /// Strongly typed access to the Authorization header.
        /// </summary>
        string Authorization { get; set; }

        /// <summary>
        /// Strongly typed access to the Accept header.
        /// </summary>
        string Accept { get; set; }

        /// <summary>
        /// Strongly typed access to the Accept-Charset header.
        /// </summary>
        string AcceptCharset { get; set; }

        /// <summary>
        /// Strongly typed access to the Accept-Encoding header.
        /// </summary>
        string AcceptEncoding { get; set; }

        /// <summary>
        /// Strongly typed access to the Accept-Language header.
        /// </summary>
        string AcceptLanguage { get; set; }

        /// <summary>
        /// Strongly typed access to the Cache-Control header.
        /// </summary>
        string CacheControl { get; set; }

        /// <summary>
        /// Strongly typed access to the Content-Disposition header.
        /// </summary>
        string ContentDisposition { get; set; }

        /// <summary>
        /// Strongly typed access to the Content-Range header.
        /// </summary>
        string ContentRange { get; set; }

        /// <summary>
        /// Strongly typed access to the Content-Type header.
        /// </summary>
        string ContentType { get; set; }

        /// <summary>
        /// Strongly typed access to the Cookie header.
        /// </summary>
        string Cookie { get; set; }

        /// <summary>
        /// Strongly typed access to the Date header.
        /// </summary>
        string Date { get; set; }

        /// <summary>
        /// Strongly typed access to the Expires header.
        /// </summary>
        string Expires { get; set; }

        /// <summary>
        /// Strongly typed access to the Host header.
        /// </summary>
        string Host { get; set; }
    }
}