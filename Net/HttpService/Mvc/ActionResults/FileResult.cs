using System;
using System.IO;

using MicroServer.Logging;
using MicroServer.Utilities;
using MicroServer.Net.Http.Files;
using MicroServer.Net.Http.Mvc.Controllers;

namespace MicroServer.Net.Http.Mvc
{
    /// <summary>
    /// Sends a binary file to the client.
    /// </summary>
    /// <remarks>Content length must be correct in order for this action to work properly.</remarks>
    public class FileResult : ActionResult
    {
        private MimeTypeProvider mimeProvider = new MimeTypeProvider();

        private string _contentDisposition;
        private string _contentType;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileResult"/> class.
        /// </summary>
        /// <param name="content">The body content.</param>
        /// <param name="fileName">The binary file name.</param>
        public FileResult(Stream content, string fileName)
        {
            if (content == null)
            {
                throw new NullReferenceException("content");
            }

            if (StringUtility.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException("filename");
            }
            
            Stream = content;
            ContentDisposition = fileName;
            _contentType = mimeProvider.Get(Path.GetFileName(fileName));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileResult"/> class.
        /// </summary>
        /// <param name="buffer">The body content</param>
        /// <param name="fileName">The binary file name.</param>
        public FileResult(byte[] buffer, string fileName)
        {
            if (buffer == null)
            {
                throw new NullReferenceException("buffer");
            }

            if (buffer.Length == 0)
            {
                throw new ArgumentOutOfRangeException("buffer");
            }

            if (StringUtility.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException("filename");
            }
            
            Stream =new MemoryStream(buffer);
            ContentDisposition = fileName;
            _contentType = mimeProvider.Get(Path.GetFileName(fileName));
        }

        /// <summary>
        /// Gets stream to send
        /// </summary>
        public Stream Stream { get; private set; }

        /// <summary>
        /// Gets content disposition file name.
        /// </summary>
        public string ContentDisposition 
        {
            get { return _contentDisposition; }
            set
            {
                _contentDisposition = "attachment; filename=\"" + Path.GetFileName(value) + "\"";
            }
        }

        /// <summary>
        /// Gets content type.
        /// </summary>
        public string ContentType 
        { 
            get { return _contentType; }
            set {  _contentType = value; }
        }

        /// <summary>
        /// Execute the response result.
        /// </summary>
        /// <param name="context">HTTP controller context</param>
        /// <remarks>Invoked by <see cref="ControllerFactory"/> to process the response.</remarks>
        public override void ExecuteResult(IControllerContext context)
        {
            Logger.WriteDebug(this, "Pipeline => ExecuteResult");
            
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            
            if (!StringUtility.IsNullOrEmpty(_contentType))
            {
                context.HttpContext.Response.ContentType = _contentType;
            }

            if (!StringUtility.IsNullOrEmpty(_contentDisposition))
            {
                context.HttpContext.Response.AddHeader("Content-Disposition", ContentDisposition);
            }

            if (Stream != null)
            {
                context.HttpContext.Response.Body = Stream;
            }
        }
    }
}