// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Bytewizer.TinyCLR.Http.Routing;
using Bytewizer.TinyCLR.Pipeline.Builder;

namespace Bytewizer.TinyCLR.Telnet
{
    /// <summary>
    /// <see cref="IApplicationBuilder"/> extension methods for the <see cref="EndpointRoutingMiddleware"/>.
    /// </summary>
    public static class CommandExtensions
    {
        /// <summary>
        /// Adds routing middleware features supporting endpoint routing.
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder"/> instance this method extends.</param>
        public static IApplicationBuilder UseCommands(this IApplicationBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var endpointRouteBuilder = new DefaultEndpointRouteBuilder(builder);
            builder.SetProperty(EndpointRouteBuilder, endpointRouteBuilder);

            return builder.UseMiddleware(typeof(EndpointRoutingMiddleware), endpointRouteBuilder);
        }

        /// <summary>
        /// Adds routing middleware features supporting endpoint routing.
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder"/> instance this method extends.</param>
        /// <param name="configure">An <see cref="EndpointRouteDelegate"/> to configure the provided <see cref="IEndpointRouteBuilder"/>.</param>
        public static IApplicationBuilder UseEndpoints(this IApplicationBuilder builder, EndpointRouteDelegate configure)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            VerifyEndpointRoutingMiddlewareIsRegistered(builder, out var endpointRouteBuilder);
            configure(endpointRouteBuilder);

            return builder.UseMiddleware(typeof(EndpointMiddleware));
        }

        private static void VerifyEndpointRoutingMiddlewareIsRegistered(IApplicationBuilder app, out DefaultEndpointRouteBuilder endpointRouteBuilder)
        {
            if (!app.TryGetProperty(EndpointRouteBuilder, out var obj))
            {
                var message =
                    $"{nameof(EndpointRoutingMiddleware)} matches endpoints setup by {nameof(EndpointMiddleware)} and so must be added to the request " +
                    $"execution pipeline before {nameof(EndpointMiddleware)}. " +
                    $"Please add {nameof(EndpointRoutingMiddleware)} by calling '{nameof(IApplicationBuilder)}.{nameof(UseRouting)}' inside the call " +
                    $"to 'Configure(...)' in the application startup code.";
                throw new InvalidOperationException(message);
            }

            // If someone messes with this, just let it crash.
            endpointRouteBuilder = (DefaultEndpointRouteBuilder)obj;

            // This check handles the case where Map or something else that forks the pipeline is called between the two
            // routing middleware.
            if (!object.ReferenceEquals(app, endpointRouteBuilder.ApplicationBuilder))
            {
                var message =
                    $"The {nameof(EndpointRoutingMiddleware)} and {nameof(EndpointMiddleware)} must be added to the same {nameof(IApplicationBuilder)} instance. " +
                    $"To use Endpoint Routing with 'Map(...)', make sure to call '{nameof(IApplicationBuilder)}.{nameof(UseRouting)}' before " +
                    $"'{nameof(IApplicationBuilder)}.{nameof(UseEndpoints)}' for each branch of the middleware pipeline.";
                throw new InvalidOperationException(message);
            }
        }
    }
}
