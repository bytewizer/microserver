// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

using Bytewizer.TinyCLR.Pipeline.Builder;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Extension methods for the <see cref="StaticFileMiddleware"/>.
    /// </summary>
    public static class FileServerExtensions 
    {
        /// <summary>
        /// Enable all static file middleware for the current request path in the root directory.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> instance this method extends.</param>
        public static IApplicationBuilder UseFileServer(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            
            return app;
        }

        /// <summary>
        /// Enable all static file middleware for the current request path in the root directory.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> instance this method extends.</param>
        /// <param name="provider">The <see cref="IContentTypeProvider"/> Uused to map files to content-types.</param>
        public static IApplicationBuilder UseFileServer(this IApplicationBuilder app, IContentTypeProvider provider)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.UseDefaultFiles();
            app.UseStaticFiles(
                    new StaticFileOptions() { ContentTypeProvider = provider }
                );

            return app;
        }
    }
}