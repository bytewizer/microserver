// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

using Bytewizer.TinyCLR.Pipeline.Builder;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Extension methods for the <see cref="CorsMiddleware"/>.
    /// </summary>
    public static class CorsMiddlewareExtensions
    {
        /// <summary>
        /// Enable cross-origin resource sharing (CORS) control capabilities.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> instance this method extends.</param>
        public static IApplicationBuilder UseCors(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware(typeof(CorsMiddleware));
        }

        /// <summary>
        /// Enable cross-origin resource sharing (CORS) control capabilities.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> instance this method extends.</param>
        /// <param name="options">The <see cref="CorsOptions"/> used to configure the middleware.</param>
        public static IApplicationBuilder UseCors(this IApplicationBuilder app, CorsOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return app.UseMiddleware(typeof(CorsMiddleware), options);
        }
    }
}