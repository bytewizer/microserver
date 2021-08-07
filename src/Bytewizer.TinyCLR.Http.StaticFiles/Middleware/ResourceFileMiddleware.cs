// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Text;
using System.Resources;
using System.Collections;

using Bytewizer.TinyCLR.Logging;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Enables serving resource files for a given request path.
    /// </summary>
    public class ResourceFileMiddleware : Middleware
    {
        private readonly ResourceFileOptions _options;
        private readonly DateTime _lastModified;
        private readonly Hashtable _resources;
        //private readonly ResourceManager _resourceManager;
        private readonly ILogger _logger;
        private readonly IContentTypeProvider _contentTypeProvider;

        /// <summary>
        /// Initializes a default instance of the <see cref="ResourceFileMiddleware"/> class.
        /// </summary>
        public ResourceFileMiddleware()
            : this (NullLoggerFactory.Instance, new ResourceFileOptions())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceFileMiddleware"/> class.
        /// </summary>
        /// <param name="options">The <see cref="ResourceFileOptions"/> used to configure the middleware.</param>
        public ResourceFileMiddleware(ResourceFileOptions options)
            : this(NullLoggerFactory.Instance, options)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceFileMiddleware"/> class.
        /// </summary>
        /// <param name="loggerFactory">The factory used to create loggers.</param>
        /// <param name="options">The <see cref="ResourceFileOptions"/> used to configure the middleware.</param>
        public ResourceFileMiddleware(ILoggerFactory loggerFactory, ResourceFileOptions options)
        {
            //var rm = new System.Resources.ResourceManager("Bytewizer.TinyCLR.WebServer.Properties.Resources", _options.Assembly);
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _logger = loggerFactory.CreateLogger("Bytewizer.TinyCLR.Http");

            _options = options;
            _resources = _options.Resources ?? new Hashtable();
            //_resourceManager = _options.ResourceManager;
            _contentTypeProvider = _options.ContentTypeProvider ?? new DefaultContentTypeProvider();
            _lastModified = DateTime.Now;

            if (_options.ResourceManager == null)
            {
                throw new ArgumentException("Missing Resource Manager implementation.");
            }
        }

        /// <summary>
        /// Processes a request to determine if it matches a known file and if so serves it.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/> that encapsulates all HTTP-specific information about an individual HTTP request.</param>
        /// <param name="next">The next request handler to be executed.</param>
        protected override void Invoke(HttpContext context, RequestDelegate next)
        {
            var matchUrl = context.Request.Path;

            if (!string.IsNullOrEmpty(matchUrl))
            {
                if (!ValidateMethod(context))
                {
                    var method = context.Request.Method;
                    _logger.InvalidRequestMethod(method);
                }
                else if (!ValidatePath(matchUrl, out string subPath, out short resourceId))
                {
                    var path = context.Request.Path;
                    _logger.InvalidRequestPath(path);
                }
                else if (!LookupContentType(_contentTypeProvider, _options, subPath, out var contentType))
                {
                    var path = context.Request.Path;
                    _logger.InvalidFileType(path);
                }
                else
                {
                    // If we get here we can try to serve the file
                    TryServeStaticFile(context, contentType, subPath, resourceId);

                    return;
                }
            }

            next(context);
        }

        private static bool ValidateMethod(HttpContext context)
        {
            if (context.Request.Method == HttpMethods.Get ||
                context.Request.Method == HttpMethods.Head)
            {
                return true;
            }

            return false;
        }

        private bool ValidatePath(string matchUrl, out string subPath, out short resourceId)
        {
            subPath = matchUrl.Split('?')[0];

            if (string.IsNullOrEmpty(subPath) || !_resources.Contains(subPath))
            {
                resourceId = 0;
                return false;
            }

            resourceId = (short)_resources[subPath];

            return true;
        }

        private void TryServeStaticFile(HttpContext context, string contentType, string subPath, short resourceId)
        {
            //TODO: Implement If-Ranged
            var modifiedSince = context.Request.Headers.IfModifiedSince;

            if (modifiedSince < _lastModified)
            {
                subPath = subPath.Replace("/", Path.DirectorySeparatorChar.ToString());

                var filename = Path.GetFileName(subPath);
                context.Response.Headers[HeaderNames.LastModified] = _lastModified.ToString("R");
                context.Response.Headers[HeaderNames.ContentDisposition] = $"inline; filename={filename}";
                context.Response.ContentType = contentType;
                context.Response.StatusCode = StatusCodes.Status200OK;

                if (context.Request.Method == HttpMethods.Get)
                {
                    // TODO:  Chunked resource using GetObject(idResource, offset, size) - GHI #761
                    var fileObject = _options.ResourceManager.GetObject(resourceId);
                    if (fileObject.GetType() == typeof(string))
                    {
                        var file = fileObject as string;
                        var fileBytes = Encoding.UTF8.GetBytes(file);
                        context.Response.Body = new MemoryStream(fileBytes);
                    }
                    else if (fileObject.GetType() == typeof(byte[]))
                    {
                        var file = fileObject as byte[];
                        context.Response.Body = new MemoryStream(file);
                    }
                    context.Response.ContentLength = context.Response.Body.Length;
                }

                return;
            }

            context.Response.StatusCode = StatusCodes.Status304NotModified;
        }

        private static bool LookupContentType(
        IContentTypeProvider contentTypeProvider,
        ResourceFileOptions options,
        string subPath,
        out string contentType)
        {
            if (contentTypeProvider.TryGetContentType(subPath, out contentType))
            {
                return true;
            }

            if (options.ServeUnknownFileTypes)
            {
                contentType = options.DefaultContentType;
                return true;
            }

            return false;
        }
    }
}