using System;
using System.Text;
using System.Collections;

using Bytewizer.TinyCLR.Http.Query;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Convenience methods for <see cref="HttpRequest"/>.
    /// </summary>
    public static class HttpRequestExtensions
    {
        /// <summary>
        /// Read a url encoded form from the request and deserialize to an <see cref="QueryCollection"/>.
        /// If the request's content-type is not a known url encoded type then <c>null</c> is returned.
        /// </summary>
        /// <param name="request">The request to read from.</param>
        public static QueryCollection ReadFromUrlEncoded(this HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
     
            if (request.HasUrlEncodedContentType())
            {
                if (request.Body.Length > 0)
                {
                    var bytes = new byte[request.Body.Length];
                    
                    request.Body.Position = 0;
                    request.Body.Read(bytes, 0, bytes.Length);
                    
                    var parsed = QueryValue.TryParseList(Encoding.UTF8.GetString(bytes), out ArrayList result);
                    if (parsed)
                    {
                        return new QueryCollection(result);
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Checks the Content-Type header for url encoded types.
        /// </summary>
        /// <returns>true if the Content-Type header represents a url encoded content type; otherwise, false.</returns>
        private static bool HasUrlEncodedContentType(this HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.Headers[HeaderNames.ContentType].Contains("application/x-www-form-urlencoded"))
            {
                return true;
            }

            return false;

        }
    }
}
