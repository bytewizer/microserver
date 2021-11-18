using System;

using Bytewizer.TinyCLR.Http.PageBuilder;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Convenience methods for <see cref="HttpResponse"/>.
    /// </summary>
    public static class HttpResponseExtensions
    {
        /// <summary>
        /// Writes the given HTML to the response body.
        /// </summary>
        /// <param name="response">The <see cref="HttpResponse"/>.</param>
        /// <param name="content">The <see cref="HtmlBody"/> document to write to the response.</param>
        public static void Write(this HttpResponse response, HtmlPage content)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            response.Write(content.ToString());
        }
    }
}