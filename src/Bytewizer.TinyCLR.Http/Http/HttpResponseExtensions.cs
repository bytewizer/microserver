using System;
using System.IO;
using System.Text;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Convenience methods for <see cref="HttpResponse"/>.
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
            response.ContentLength = response.ContentLength += encoded.Length;
            response.Body.Position = 0;
            response.Body.Write(encoded, 0, encoded.Length);
        }

        /// <summary>
        /// Returns a redirect response (HTTP 301, HTTP 302, HTTP 307 or HTTP 308) to the client.
        /// </summary>
        /// <param name="response">The <see cref="HttpResponse"/> to redirect.</param>
        /// <param name="location">The URL to redirect the client to. This must be properly encoded for use in http headers where only ASCII characters are allowed.</param>
        /// <param name="permanent"><c>True</c> if the redirect is permanent (301 or 308), otherwise <c>false</c> (302 or 307).</param>
        /// <param name="preserveMethod"><c>True</c> if the redirect needs to reuse the method and body (308 or 307), otherwise <c>false</c> (301 or 302).</param>
        public static void Redirect(this HttpResponse response, string location, bool permanent, bool preserveMethod)
        {
            if (preserveMethod)
            {
                response.StatusCode = permanent ? StatusCodes.Status308PermanentRedirect : StatusCodes.Status307TemporaryRedirect;
            }
            else
            {
                response.StatusCode = permanent ? StatusCodes.Status301MovedPermanently : StatusCodes.Status302Found;
            }

            response.Headers[HeaderNames.Location] = location;
        }

        /// <summary>
        /// Sends the given file to the response body.
        /// </summary>
        /// <param name="response">The <see cref="HttpResponse"/>.</param>
        /// <param name="fullPath">The full path to the file.</param>
        /// <param name="contentType">The Content-Type header of the file response.</param>
        public static void SendFile(this HttpResponse response, string fullPath, string contentType)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            var fileName = Path.GetFileName(fullPath);
            var fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);

            response.SendFile(fileStream, contentType, fileName);
        }

        /// <summary>
        /// Sends the given file to the response body.
        /// </summary>
        /// <param name="response">The <see cref="HttpResponse"/>.</param>
        /// <param name="fileStream">The full path to the file.</param>
        /// <param name="contentType">The Content-Type header of the file response.</param>
        /// <param name="fileName">The file name that will be used in the Content-Disposition header of the response.</param>
        public static void SendFile(this HttpResponse response, Stream fileStream, string contentType, string fileName)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            if (fileStream == null)
            {
                throw new ArgumentNullException(nameof(fileStream));
            }

            if (string.IsNullOrEmpty(contentType))
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            response.Headers[HeaderNames.ContentDisposition] = $"inline; filename={fileName}";
            response.ContentType = contentType;
            response.Body = fileStream;
            response.ContentLength = fileStream.Length;
            response.StatusCode = StatusCodes.Status200OK;
        }
    }
}