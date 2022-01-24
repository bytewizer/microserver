// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Enables file mapping for Blazor WebAssembly applications.
    /// </summary>
    public class BlazorFrameworkMiddleware : Middleware
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlazorFrameworkMiddleware"/> class.
        /// </summary>
        public BlazorFrameworkMiddleware()
        {
        }

        /// <inheritdoc/>
        protected override void Invoke(HttpContext context, RequestDelegate next)
        {
            if (context.Request.Path.StartsWith("/_framework"))
            {
                context.Response.Headers[HeaderNames.CacheControl] = "no-cache";
                context.Response.Headers[HeaderNames.ContentEncoding] = "gzip"; // "gzip" "br"
                context.Response.Headers[HeaderNames.Vary] = HeaderNames.ContentEncoding;
                context.Request.Path = context.Request.Path + ".gz"; // ".gz" ".br" note: br only supported in https
            }

            next(context);
        }
    }
}