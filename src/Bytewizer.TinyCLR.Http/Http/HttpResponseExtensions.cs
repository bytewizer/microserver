using System;
using System.Text;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Convenience methods for writing to the response.
    /// </summary>
    public static class HttpResponseExtensions
    {
        /// <summary>
        /// Writes the given text to the response body. UTF-8 encoding will be used.
        /// </summary>
        /// <param name="response">The <see cref="HttpResponse"/>.</param>
        /// <param name="text">The text to write to the response.</param>
        public static void Write(this HttpResponse response, string text)
        {
            Write(response, text, "text/html; charset=UTF-8", StatusCodes.Status200OK, Encoding.UTF8);
        }

        /// <summary>
        /// Writes the given text to the response body. UTF-8 encoding will be used.
        /// </summary>
        /// <param name="response">The <see cref="HttpResponse"/>.</param>
        /// <param name="text">The text to write to the response.</param>
        /// <param name="contentType">The content MIME type.</param>
        public static void Write(this HttpResponse response, string text, string contentType)
        {
            Write(response, text, contentType, StatusCodes.Status200OK, Encoding.UTF8);
        }

        /// <summary>
        /// Writes the given text to the response body using the given encoding.
        /// </summary>
        /// <param name="response">The <see cref="HttpResponse"/>.</param>
        /// <param name="text">The text to write to the response.</param>
        /// <param name="contentType">The content MIME type.</param>
        /// <param name="statusCode">The status code to set on the response.</param>
        /// <param name="encoding">The encoding to use.</param>
        public static void Write(this HttpResponse response, string text, string contentType, int statusCode, Encoding encoding)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (contentType == null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            var encoded = encoding.GetBytes(text);
            response.StatusCode = statusCode;
            response.ContentType = contentType;
            response.ContentLength += encoded.Length;
            response.Body.Position = 0;
            response.Body.Write(encoded, 0, encoded.Length);
        }
    }
}