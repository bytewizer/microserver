using System;
using System.Text;

using MicroServer.Logging;
using MicroServer.Utilities;
using MicroServer.Net.Http.Files;
using MicroServer.Net.Http.Mvc.Controllers;
using System.IO;

namespace MicroServer.Net.Http.Mvc.ActionResults
{ 
    /// <summary>
    /// Send binary content to the client. 
    /// </summary>
    public class ContentResult : ActionResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentResult"/> class.
        /// </summary>
        /// <param name="content">The body content.</param>
        /// <param name="contentType">The content type header.</param>
        public ContentResult(Stream content, string contentType)
        {
            if (content == null)
            {
                throw new NullReferenceException("stream");
            }

            if (StringUtility.IsNullOrEmpty(contentType))
            {
                throw new ArgumentException("contentType");
            }

            Stream = content;
            ContentType = contentType;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentResult"/> class.
        /// </summary>
        /// <param name="buffer">The body content.</param>
        /// <param name="contentType">The content type header.</param>
        public ContentResult(byte[] buffer, string contentType)
        {
            if (buffer.Length == 0)
            {
                throw new ArgumentOutOfRangeException("buffer");
            }

            if (StringUtility.IsNullOrEmpty(contentType))
            {
                throw new ArgumentException("contentType");
            }
            
            Stream = new MemoryStream(buffer);
            ContentType = contentType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentResult"/> class.
        /// </summary>
        /// <param name="content">The body content.</param>
        /// <param name="contentType">The content type header.</param>
        public ContentResult(string content, string contentType)
        {
            if (StringUtility.IsNullOrEmpty(content))
            {
                throw new ArgumentException("content");
            }

            if (StringUtility.IsNullOrEmpty(contentType))
            {
                throw new ArgumentException("contentType");
            }
            
            Stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            ContentType = contentType;
        }

        /// <summary>
        /// Gets stream to send
        /// </summary>
        public Stream Stream { get; private set; }

        /// <summary>
        /// Gets or sets content type.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Execute the response result.
        /// </summary>
        /// <param name="context">HTTP controller context</param>
        /// <remarks>Invoked by <see cref="ControllerFactory"/> to process the response.</remarks>
        public override void ExecuteResult(IControllerContext context)
        {

            //Logger.WriteDebug(this, "Pipeline => ExecuteResult");
            
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (!StringUtility.IsNullOrEmpty(ContentType))
            {
                context.HttpContext.Response.ContentType = ContentType;
            }

            if (Stream != null)
            {
                context.HttpContext.Response.Body = Stream;
            }
        }
    }
}