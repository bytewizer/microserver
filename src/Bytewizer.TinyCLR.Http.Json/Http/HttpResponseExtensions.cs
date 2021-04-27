using System;
using System.Text;

using GHIElectronics.TinyCLR.Data.Json;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Convenience methods for <see cref="HttpResponse"/>.
    /// </summary>
    public static class HttpResponseExtensions
    {
        private static readonly string _contentType = "application/json; charset=UTF-8";

        /// <summary>
        /// Writes the given JavaScript Object Notation (JSON) to the response body. UTF-8 encoding will be used.
        /// </summary>
        /// <param name="response">The <see cref="HttpResponse"/>.</param>
        /// <param name="value">The value to format as JSON and write to the response.</param>
        /// <param name="indented">The value that defines whether JSON should use pretty printing.</param>
        public static void WriteJson(this HttpResponse response, string value, bool indented = false)
        {
            WriteJson(response, value, _contentType, StatusCodes.Status200OK, Encoding.UTF8, new JsonSerializationOptions() { Indented = indented });
        }

        /// <summary>
        /// Writes the given JavaScript Object Notation (JSON) to the response body. UTF-8 encoding will be used.
        /// </summary>
        /// <param name="response">The <see cref="HttpResponse"/>.</param>
        /// <param name="value">The value to format as JSON and write to the response.</param>
        /// <param name="indented">The value that defines whether JSON should use pretty printing.</param>
        public static void WriteJson(this HttpResponse response, object value, bool indented = false)
        {
            WriteJson(response, value, _contentType, StatusCodes.Status200OK, Encoding.UTF8, new JsonSerializationOptions() { Indented = indented });
        }

        /// <summary>
        /// Writes the given JavaScript Object Notation (JSON) to the response body using the given encoding.
        /// </summary>
        /// <param name="response">The <see cref="HttpResponse"/>.</param>
        /// <param name="value">The text to write to the response.</param>
        /// <param name="contentType">The content MIME type.</param>
        /// <param name="statusCode">The status code to set on the response.</param>
        /// <param name="encoding">The encoding to use.</param>
        /// <param name="settings">Specifies the settings on a JsonSerializer object.</param> 
        public static void WriteJson(this HttpResponse response, object value, string contentType, int statusCode, Encoding encoding, JsonSerializationOptions options,  JsonSerializerSettings settings = null)
        {

            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (string.IsNullOrEmpty(contentType))
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var encoded = encoding.GetBytes(JsonConverter.Serialize(value, settings).ToString(options));
            response.StatusCode = statusCode;
            response.ContentType = contentType;
            response.ContentLength = encoded.Length; //response.ContentLength += encoded.Length;
            response.Body.Position = 0;
            response.Body.Write(encoded, 0, encoded.Length);
        }
    }
}