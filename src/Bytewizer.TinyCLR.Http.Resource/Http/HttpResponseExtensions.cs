using System;
using System.Resources;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Convenience methods for <see cref="HttpResponse"/>.
    /// </summary>
    public static class HttpResponseExtensions
    {
        /// <summary>
        /// Sends the given <see cref="ResourceManager"/> object to the response body.
        /// </summary>
        /// <param name="response">The <see cref="HttpResponse"/>.</param>
        /// <param name="resourceId">The resource id to send.</param>
        /// <param name="contentType">The Content-Type header of the file response.</param>
        /// <param name="fileName">The file name that will be used in the Content-Disposition header of the response.</param>
        public static void SendResource(this HttpResponse response, short resourceId, string contentType, string fileName)
        {            
            var resourceManager = response.HttpContext.GetResourceManager();
            if (resourceManager == null)
            {
                throw new NullReferenceException("UseManager middleware implementation missing");
            }

            var resrouceStream = new ResourceStream(resourceManager, resourceId);

            response.SendResource(resrouceStream, contentType, fileName);
        }

        /// <summary>
        /// Sends the given <see cref="ResourceStream"/> to the response body.
        /// </summary>
        /// <param name="response">The <see cref="HttpResponse"/>.</param>
        /// <param name="resourceStream">The resource stream to send.</param>
        /// <param name="contentType">The Content-Type header of the file response.</param>
        /// <param name="fileName">The file name that will be used in the Content-Disposition header of the response.</param>
        public static void SendResource(this HttpResponse response, ResourceStream resourceStream, string contentType, string fileName)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            if (resourceStream == null)
            {
                throw new ArgumentNullException(nameof(resourceStream));
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
            response.Body = resourceStream;
            response.ContentLength = resourceStream.Length; 
            response.StatusCode = StatusCodes.Status200OK;
        }
    }
}