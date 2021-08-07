// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

using Bytewizer.TinyCLR.Http.Features;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// A middleware for generating the response body of error status codes with no body.
    /// </summary>
    public class StatusCodePagesMiddleware : Middleware
    {
        private readonly StatusCodePagesOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusCodePagesMiddleware"/> class.
        /// </summary>
        public StatusCodePagesMiddleware()
        {
            _options = new StatusCodePagesOptions();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusCodePagesMiddleware"/> class.
        /// </summary>
        /// <param name="options">The options for configuring the middleware.</param>
        public StatusCodePagesMiddleware(StatusCodePagesOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            _options = options;

            if (_options.Handle == null)
            {
                throw new ArgumentException("Missing options.HandleAsync implementation.");
            }
        }

        /// <inheritdoc/>
        protected override void Invoke(HttpContext context, RequestDelegate next)
        {
            var statusCodeFeature = new StatusCodePagesFeature();
            context.Features.Set(typeof(IStatusCodePagesFeature), statusCodeFeature);

            next(context);

            statusCodeFeature = (StatusCodePagesFeature)context.Features.Get(typeof(IStatusCodePagesFeature));
            if (!statusCodeFeature.Enabled)
            {
                // Check if the feature is still available because other middleware could
                // have disabled the feature to prevent HTML status code responses from showing up.
                return;
            }

            if (context.Response.StatusCode < 400
                || context.Response.StatusCode >= 600
                || context.Response.ContentLength > 0
                || !string.IsNullOrEmpty(context.Response.ContentType))
            {
                return;
            }

            _options.Handle(context);
        }
    }
}