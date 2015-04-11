using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;

using MicroServer.Net.Http.Modules;
using MicroServer.Utilities;
using MicroServer.Extensions;
using System.Collections;
using Microsoft.SPOT;

namespace MicroServer.Net.Http.Files
{
    /// <summary>
    /// Will serve static files
    /// </summary>
    /// <example>
    /// <code>
    /// // One of the available file services.
    /// var diskFiles = new DiskFileService("/public/", @"C:\www\public\");
    /// var module = new FileModule(diskFiles);
    /// 
    /// // the module manager is added to the HttpServer.
    /// var moduleManager = new ModuleManager();
    /// moduleManager.Add(module);
    /// </code>
    /// </example>
    public class FileModule : IWorkerModule
    {
        private readonly IFileService _fileService;

        private const string ListingTemplate =
            @"<!DOCTYPE html><html style='font-family: sans-serif;'><head><title>Listing files</title><head><body><table style='padding:15px;'><thead>" +
            @"<tr><th style='text-align: left;'>Name</th><th style='text-align: left;'>Last modifiled</th><th style='text-align: left;'>Size</th></tr></thead> " + 
            @"<tbody>{{Files}}</tbody></table></body></html>";

        /// <summary>
        /// Initializes a new instance of the <see cref="FileModule" /> class.
        /// </summary>
        /// <param name="fileService">The file service.</param>
        public FileModule(IFileService fileService)
        {
            if (fileService == null) throw new ArgumentNullException("fileService");
            _fileService = fileService;

            ListingHtml = ListingTemplate;
        }

        /// <summary>
        /// Gets or sets if we should allow file listing
        /// </summary>
        public bool AllowListing { get; set; }

        /// <summary>
        /// Template which is used to list files. Should be a complete HTML page where <c>{{Files}}</c> will be replaced with a number of table rows.
        /// </summary>
        public string ListingHtml { get; set; }

        #region IWorkerModule Members

        /// <summary>
        /// Invoked before anything else
        /// </summary>
        /// <param name="context">HTTP context</param>
        /// <remarks>
        /// <para>The first method that is exeucted in the pipeline.</para>
        /// Try to avoid throwing exceptions if you can. Let all modules have a chance to handle this method. You may break the processing in any other method than the Begin/EndRequest methods.
        /// <para>If you are going to handle the request, implement <see cref="IWorkerModule"/> and do it in the <see cref="IWorkerModule.HandleRequest"/> method.</para>
        /// </remarks>
        public void BeginRequest(IHttpContext context)
        {
        }

        /// <summary>
        /// End request is typically used for post processing. The response should already contain everything required.
        /// </summary>
        /// <param name="context">HTTP context</param>
        /// <remarks>
        /// <para>The last method that is executed in the pipeline.</para>
        /// Try to avoid throwing exceptions if you can. Let all modules have a chance to handle this method. You may break the processing in any other method than the Begin/EndRequest methods.</remarks>
        public void EndRequest(IHttpContext context)
        {
        }

        /// <summary>
        /// Handle the request asynchronously.
        /// </summary>
        /// <param name="context">HTTP context</param>
        /// <param name="callback">callback</param>
        /// <returns><see cref="ModuleResult.Stop"/> will stop all processing except <see cref="IHttpModule.EndRequest"/>.</returns>
        /// <remarks>Invoked in turn for all modules unless you return <see cref="ModuleResult.Stop"/>.</remarks>
        public void HandleRequestAsync(IHttpContext context, AsyncModuleHandler callback)
        {
            callback(new AsyncModuleResult(context, HandleRequest(context)));
        }

        /// <summary>
        /// Handle the request.
        /// </summary>
        /// <param name="context">HTTP context</param>
        /// <returns><see cref="ModuleResult.Stop"/> will stop all processing except <see cref="IHttpModule.EndRequest"/>.</returns>
        /// <remarks>Invoked in turn for all modules unless you return <see cref="ModuleResult.Stop"/>.</remarks>
        public ModuleResult HandleRequest(IHttpContext context)
        {
            // only handle GET and HEAD
            if (!context.Request.HttpMethod.ToUpper().Equals("GET")
                && !context.Request.HttpMethod.ToUpper().Equals("HEAD"))
                return ModuleResult.Continue;

            // serve a directory
            if (AllowListing)
            {
                if (TryGenerateDirectoryPage(context))
                    return ModuleResult.Stop;
            }

            var header = context.Request.Headers["If-Modified-Since"];
            
            // TODO: Build reliable date parser
            var time = DateTime.MinValue;
            //var time = header != null
            //               ? ParseUtility.TryParseDateTime(header)
            //               : DateTime.MinValue;
           
            var fileContext = new FileContext(context.Request, time);
            _fileService.GetFile(fileContext);
            if (!fileContext.IsFound)
                return ModuleResult.Continue;

            if (!fileContext.IsModified)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotModified;
                context.Response.StatusDescription = "Was last modified " + fileContext.LastModifiedAtUtc.ToString("R");
                return ModuleResult.Stop;
            }

            var mimeType = MimeTypeProvider.Instance.Get(fileContext.Filename);
            if (mimeType == null)
            {
                context.Response.StatusCode = (int)HttpStatusCode.UnsupportedMediaType;
                context.Response.StatusDescription = string.Concat("File type '", Path.GetExtension(fileContext.Filename),"' is not supported.");
                return ModuleResult.Stop;
            }

            context.Response.AddHeader("Last-Modified", fileContext.LastModifiedAtUtc.ToString("R"));
            context.Response.AddHeader("Accept-Ranges", "bytes");
            context.Response.AddHeader("Content-Disposition", "inline;filename=\"" + Path.GetFileName(fileContext.Filename) + "\"");
            context.Response.ContentType = mimeType;
            context.Response.ContentLength = (int)fileContext.FileStream.Length;
            context.Response.Body = fileContext.FileStream;

            // Do not include a body when the client only want's to get content information.
            if (context.Request.HttpMethod.ToUpper().Equals("HEAD") && context.Response.Body != null)
            {
                context.Response.Body.Dispose();
                context.Response.Body = null;
            }
            return ModuleResult.Stop;
        }

        /// <summary>
        /// Creates an Html directory listing including files and directories  
        /// </summary>
        /// <param name="context">Context used to locate and return files</param>
        private bool TryGenerateDirectoryPage(IHttpContext context)
        {
            if (!_fileService.IsDirectory(context.Request.Uri))
                return false;

            int pos = ListingHtml.IndexOf("{{Files}}");
            if (pos == -1)
                throw new InvalidOperationException("Failed to find '{{Files}}' in the ListFilesTemplate.");

            var newLine = ListingHtml.LastIndexOf("\r\n", pos);
            
            var sb = new StringBuilder();
            sb.Append(ListingHtml.Substring(0, pos));

            string ParentUri = "/";
            int ParentPos = context.Request.Uri.AbsolutePath.LastIndexOf("/");
            if (ParentPos > 0)
                ParentUri = context.Request.Uri.AbsolutePath.Substring(0, ParentPos);
  
            if (!context.Request.Uri.AbsolutePath.Equals("/"))
            {
                sb.Append("<tr>");
                sb.Append("<td><a href='" + ParentUri + "'>parent directory/</a></td>");
                sb.Append("<td>-</td>");
                sb.Append("<td>-</td>");
                sb.Append("</tr>");
            }

            foreach (DirectoryInformation directory in _fileService.GetDirectories(context.Request.Uri))
            {
                var DirectoryUri = context.Request.Uri.AbsolutePath;
                if (!DirectoryUri.EndsWith("/"))
                    DirectoryUri += "/";

                DirectoryUri += directory.Name;

                sb.Append("<tr>");
                sb.Append("<td><a href='" + DirectoryUri + "'>" + directory.Name.ToLower() + "/</a></td>");
                sb.Append("<td>" + directory.LastModifiedAtUtc.ToString("g") + "</td>");
                sb.Append("<td>-</td>");
                sb.Append("</tr>");
            }

            foreach (FileInformation file in _fileService.GetFiles(context.Request.Uri))
            {
                var fileUri = context.Request.Uri.AbsolutePath;
                if (!fileUri.EndsWith("/"))
                    fileUri += "/";
            
                fileUri += file.Name;

                sb.Append("<tr>");
                sb.Append("<td><a href='" + fileUri + "'>" + file.Name.ToLower() + "</a></td>");
                sb.Append("<td>" + file.LastModifiedAtUtc.ToString("g") + "</td>");
                sb.Append("<td>" + StringUtility.FormatDiskSize(file.Size) + "</td>");
                sb.Append("</tr>");
            }

            sb.Append(ListingHtml.Substring(pos + 9));
            context.Response.Body = new MemoryStream(Encoding.UTF8.GetBytes(sb.ToString()));
            
            return true;
        }
       
        #endregion
    }
}