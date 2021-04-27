using System;

using GHIElectronics.TinyCLR.Data.Json;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Convenience methods for <see cref="HttpRequest"/>.
    /// </summary>
    public static class HttpRequestExtensions
    {
        /// <summary>
        /// Read JSON from the request and deserialize to the specified type.
        /// If the request's content-type is not a known JSON type then an error will be thrown.
        /// </summary>
        /// <param name="request">The request to read from.</param>
        /// <param name="type">The type of object to read.</param>
        /// <param name="factory">The type of object to read.</param>
        public static object ReadFromJson(this HttpRequest request, Type type, InstanceFactory factory = null)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (request.HasJsonContentType())
            {
                if (request.Body.Length > 0)
                {
                    request.Body.Position = 0;
                    return JsonConverter.DeserializeObject(request.Body, type, factory);
                }
            }

            return null;
        }

        /// <summary>
        /// Checks the Content-Type header for JSON types.
        /// </summary>
        /// <returns>true if the Content-Type header represents a JSON content type; otherwise, false.</returns>
        private static bool HasJsonContentType(this HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.Headers[HeaderNames.ContentType].Contains("application/json"))
            {
                return true;
            }

            return false;

        }
    }
}
