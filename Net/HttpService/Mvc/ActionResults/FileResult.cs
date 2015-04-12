using System;
using System.IO;

using MicroServer.Logging;
using MicroServer.Utilities;
using MicroServer.Net.Http.Files;
using MicroServer.Net.Http.Mvc.Controllers;
using System.Net;

namespace MicroServer.Net.Http.Mvc.ActionResults
{
    /// <summary>
    /// Sends a binary file to the client.
    /// </summary>
    public class FileResult : ActionResult
    {
        private readonly IFileService _fileService;
        private readonly string _fullFilePath; 

        /// <summary>
        /// Initializes a new instance of the <see cref="FileResult" /> class.
        /// </summary>
        /// <param name="fileNamePath">Path to serve files from.</param>
        public FileResult(string rootFilePath, string fileNamePath)
        {
            _fileService = new DiskFileService("/", rootFilePath);
            _fullFilePath = rootFilePath + fileNamePath;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileResult" /> class.
        /// </summary>
        /// <param name="rootUri">Serve all files which are located under this URI</param>
        /// <param name="rootFilePath">Path to serve files from.</param>
        /// <param name="fileNamePath">Path and file name used to locate files</param>
        public FileResult(string rootUri, string rootFilePath, string fileNamePath)
        {
            _fileService = new DiskFileService(rootUri, rootFilePath);
            _fullFilePath = rootFilePath + fileNamePath;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileResult" /> class.
        /// </summary>
        /// <param name="fileService">Used to locate file.</param>
        /// <param name="context">Context used to locate and return files</param>
        /// <param name="fullFilePath">Full path used to locate files</param>
        public FileResult(IFileService fileService, string fullFilePath)
        {
            _fileService = fileService;
            _fullFilePath = fullFilePath;
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
            
            // only handle GET and HEAD
            if (!context.HttpContext.Request.HttpMethod.ToUpper().Equals("GET")
                && !context.HttpContext.Request.HttpMethod.ToUpper().Equals("HEAD"))
                return;
            
            var header = context.HttpContext.Request.Headers["If-Modified-Since"];

            // TODO: Build reliable date parser
            var time = DateTime.MinValue;
            //var time = header != null
            //               ? ParseUtility.TryParseDateTime(header)
            //               : DateTime.MinValue;

            var fileContext = new FileContext(context.HttpContext.Request, time);
            _fileService.GetFile(fileContext,_fullFilePath);
            if (!fileContext.IsFound)
                return;

            if (!fileContext.IsModified)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotModified;
                context.HttpContext.Response.StatusDescription = "Was last modified " + fileContext.LastModifiedAtUtc.ToString("R");
                return;
            }

            var mimeType = MimeTypeProvider.Instance.Get(fileContext.Filename);
            if (mimeType == null)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.UnsupportedMediaType;
                context.HttpContext.Response.StatusDescription = string.Concat("File type '", Path.GetExtension(fileContext.Filename), "' is not supported.");
                return;
            }

            context.HttpContext.Response.AddHeader("Last-Modified", fileContext.LastModifiedAtUtc.ToString("R"));
            context.HttpContext.Response.AddHeader("Accept-Ranges", "bytes");
            context.HttpContext.Response.AddHeader("Content-Disposition", "inline;filename=\"" + Path.GetFileName(fileContext.Filename) + "\"");
            context.HttpContext.Response.ContentType = mimeType;
            context.HttpContext.Response.ContentLength = (int)fileContext.FileStream.Length;
            context.HttpContext.Response.Body = fileContext.FileStream;

            // Do not include a body when the client only want's to get content information.
            if (context.HttpContext.Request.HttpMethod.ToUpper().Equals("HEAD") && context.HttpContext.Response.Body != null)
            {
                context.HttpContext.Response.Body.Dispose();
                context.HttpContext.Response.Body = null;
            }
        }
    }
}