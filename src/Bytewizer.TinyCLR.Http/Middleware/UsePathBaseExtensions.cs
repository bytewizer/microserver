// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

using Bytewizer.TinyCLR.Pipeline.Builder;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Extension methods for <see cref="IApplicationBuilder"/>.
    /// </summary>
    public static class UsePathBaseExtensions
    {
        /// <summary>
        /// Adds a middleware that extracts the specified path base from request path and postpend it to the request path base.
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder"/> instance.</param>
        /// <param name="pathBase">The path base to extract.</param>
        /// <returns>The <see cref="IApplicationBuilder"/> instance.</returns>
        public static IApplicationBuilder UsePathBase(this IApplicationBuilder builder, string pathBase)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            // Strip trailing slashes
            if (string.IsNullOrEmpty(pathBase))
            {
                return builder;
            }

            return builder.UseMiddleware(typeof(UsePathBaseMiddleware), pathBase);
        }
    }
}