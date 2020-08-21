using System;
using System.Text;

namespace Bytewizer.TinyCLR.Http
{
    public static class HttpResponseExtensions
    {
        public static void Write(this HttpResponse response, string text)
        {
            Write(response, text, "text/html; charset=UTF-8", StatusCodes.Status200OK, Encoding.UTF8);
        }

        public static void Write(this HttpResponse response, string text, string contentType)
        {
            Write(response, text, contentType, StatusCodes.Status200OK, Encoding.UTF8);
        }

        public static void Write(this HttpResponse response,  string text, string contentType, int status, Encoding encoding)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }
           
            var encoded = encoding.GetBytes(text);
            response.StatusCode = status;
            response.ContentType = contentType;
            response.ContentLength += encoded.Length;
            response.Body.Write(encoded, 0, encoded.Length);
        }
    }
}
