// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

using Bytewizer.TinyCLR.Pipeline.Builder;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// <see cref="IApplicationBuilder"/> extension methods for the <see cref="BlazorFrameworkMiddleware"/>.
    /// </summary>
    public static class BlazorFrameworkExtensions
    {
        /// <summary>
        /// Configures the application to serve Blazor WebAssembly framework files from the path <paramref name="pathPrefix"/>. 
        /// This path must correspond to a referenced Blazor WebAssembly application project.
        /// </summary>
        /// <param name="pathPrefix">The path string that indicates the prefix for the Blazor WebAssembly application.</param>
        /// <param name="builder">The <see cref="IApplicationBuilder"/> instance this method extends.</param>
        public static IApplicationBuilder UseBlazorFrameworkFiles(this IApplicationBuilder builder, string pathPrefix)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.UseMiddleware(typeof(BlazorFrameworkMiddleware));

            var contentTypeProvider = new DefaultContentTypeProvider();
            contentTypeProvider.AddMapping(".wasm", "application/wasm");
            contentTypeProvider.AddMapping(".pdb", "application/octet-stream");
            contentTypeProvider.AddMapping(".br", "application/x-br");
            contentTypeProvider.AddMapping(".dat", "application/octet-stream");
            contentTypeProvider.AddMapping(".blat", "application/octet-stream");
            contentTypeProvider.AddMapping(".dll", "application/octet-stream");

            var options = new StaticFileOptions()
            {
                ContentTypeProvider = contentTypeProvider
            };

            builder.UsePathBase(pathPrefix);
            builder.UseDefaultFiles();
            builder.UseStaticFiles(options);

            return builder;
        }

        private static void AddMapping(this DefaultContentTypeProvider provider, string name, string mimeType)
        {
            if (!provider.Mappings.Contains(name))
            {
                provider.Mappings.Add(name, mimeType);
            }
        }
    }
}