using System;
using System.Collections;

using Bytewizer.TinyCLR.Sockets;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Enables serving resource files for a given request path.
    /// </summary>
    public class ResourceFileMiddleware : Middleware
    {
        private readonly ResourceFileOptions _options;
        private readonly Hashtable _resources;
        private readonly IContentTypeProvider _contentTypeProvider;

        /// <summary>
        /// Initializes a default instance of the <see cref="ResourceFileMiddleware"/> class.
        /// </summary>
        public ResourceFileMiddleware()
            : this (new ResourceFileOptions())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceFileMiddleware"/> class.
        /// </summary>
        /// <param name="options">The <see cref="ResourceFileOptions"/> used to configure the middleware.</param>
        public ResourceFileMiddleware(ResourceFileOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            _options = options;
            _resources = _options.Resources ?? new Hashtable();
            _contentTypeProvider = _options.ContentTypeProvider ?? new DefaultContentTypeProvider();
        }

        /// <summary>
        /// Processes a request to determine if it matches a known file and if so serves it.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/> that encapsulates all HTTP-specific information about an individual HTTP request.</param>
        protected override void Invoke(HttpContext context, RequestDelegate next)
        {
            throw new NotImplementedException();
            
            //var matchUrl = context.Request.Path;
            //if (!string.IsNullOrEmpty(matchUrl))
            //{
            //    if (!ValidateEndpoint(context))
            //    {
            //        Debug.WriteLine("Static files was skipped as the request already matched an endpoint");
            //    }
            //    else if (!ValidateMethod(context))
            //    {
            //        var method = context.Request.Method;
            //        Debug.WriteLine($"The request method {method} are not supported");
            //    }
            //    else if (!ValidatePath(matchUrl, out short resourceId))
            //    {
            //        var path = context.Request.Path;
            //        Debug.WriteLine($"The request path {path} does not match the path filter");
            //    }
            //}

            //next(context);
        }

        //private static bool ValidateEndpoint(HttpContext context)
        //{
        //    if (context.GetEndpoint() == null)
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        //private static bool ValidateMethod(HttpContext context)
        //{
        //    if (context.Request.Method == HttpMethods.Get ||
        //        context.Request.Method == HttpMethods.Head)
        //    {
        //        return true;
        //    }

        //    return false;
        //}

        //private bool ValidatePath(string matchUrl, out short resourceId)
        //{
        //    var subPath = matchUrl.Split('?')[0];

        //    if (string.IsNullOrEmpty(subPath) || !_resources.Contains(subPath))
        //    {
        //        resourceId = 0;
        //        return false;
        //    }

        //    resourceId = (short)_resources[subPath];

           
        //    return true;
        //}
    }
}