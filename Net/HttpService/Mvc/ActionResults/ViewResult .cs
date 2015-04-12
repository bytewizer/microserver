using System;
using System.Net;
using System.IO;
using System.Text;

using Microsoft.SPOT;

using MicroServer.Logging;
using MicroServer.Utilities;
using MicroServer.Extensions;
using MicroServer.Net.Http.Files;
using MicroServer.Net.Http.Mvc;
using MicroServer.Net.Http.Mvc.Views;
using MicroServer.Net.Http.Mvc.Controllers;

namespace MicroServer.Net.Http.Mvc.ActionResults
{
    /// <summary>
    /// Sends a binary file to the client.
    /// </summary>
    public class ViewResult : ActionResult
    {
        private readonly IFileService _fileService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewResult" /> class.
        /// </summary>
        public ViewResult()
        {
            _fileService = new DiskFileService("/", @"\winfs\"+ Constants.HTTP_WEB_ROOT_FOLDER + @"\");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewResult" /> class.
        /// </summary>
        /// <param name="rootUri">Serve all files which are located under this URI</param>
        /// <param name="rootFilePath">Path to serve files from.</param>
        /// <param name="fileNamePath">Path and file name used to locate files</param>
        public ViewResult(string rootUri, string rootFilePath, string fileNamePath)
        {
            _fileService = new DiskFileService(rootUri, rootFilePath);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewResult" /> class.
        /// </summary>
        /// <param name="fileService">Used to locate file.</param>
        /// <param name="context">Context used to locate and return files</param>
        public ViewResult(IFileService fileService)
        {
            _fileService = fileService;
        }

        /// <summary>
        /// Execute the response result.
        /// </summary>
        /// <param name="context">HTTP controller context</param>
        /// <remarks>Invoked by <see cref="ControllerFactory"/> to process the response.</remarks>
        public override void ExecuteResult(IControllerContext context)
        {           
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
            string fileNamePath = string.Concat(@"\winfs\", 
                                    Constants.HTTP_WEB_ROOT_FOLDER, @"\",
                                    Constants.HTTP_WEB_VIEWS_FOLDER, @"\",
                                    context.ControllerName.Replace("Controller", String.Empty), @"\",
                                    context.ActionName, @".html");

            _fileService.GetFile(fileContext, fileNamePath);
            if (!fileContext.IsFound)
            { 
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.HttpContext.Response.StatusDescription = string.Concat("Failed to find view '", fileContext.Filename, ".");
                return;      
            }

            var mimeType = MimeTypeProvider.Instance.Get(fileContext.Filename);
            if (mimeType == null || mimeType != "text/html")
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.UnsupportedMediaType;
                context.HttpContext.Response.StatusDescription = string.Concat("File type '", Path.GetExtension(fileContext.Filename), "' is not supported.");
                return;
            }

            context.HttpContext.Response.AddHeader("Last-Modified", fileContext.LastModifiedAtUtc.ToString("R"));
            context.HttpContext.Response.AddHeader("Accept-Ranges", "bytes");
            context.HttpContext.Response.AddHeader("Content-Disposition", "inline;filename=\"" + Path.GetFileName(fileContext.Filename) + "\"");
            context.HttpContext.Response.ContentType = mimeType;

            //  Parse tokens from file content
            using (StreamReader sr = new StreamReader(fileContext.FileStream))
            {
                TokenEngine content = new TokenEngine(sr.ReadToEnd(), context.ViewData);
                MemoryStream body = new MemoryStream(Encoding.UTF8.GetBytes(content.ToString()));
                context.HttpContext.Response.Body = body;
                context.HttpContext.Response.ContentLength = (int)body.Length;
            }

            // Do not include a body when the client only want's to get content information.
            if (context.HttpContext.Request.HttpMethod.ToUpper().Equals("HEAD") && context.HttpContext.Response.Body != null)
            {
                context.HttpContext.Response.Body.Dispose();
                context.HttpContext.Response.Body = null;
            }
        }
    }
}