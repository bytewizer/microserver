// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Bytewizer.TinyCLR.Logging;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Enables endpoint routing features for incoming requests.
    /// </summary>
    public class EndpointMiddleware : Middleware
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="EndpointMiddleware"/> class.
        /// </summary>
        public EndpointMiddleware()
            :this(NullLoggerFactory.Instance)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EndpointMiddleware"/> class.
        /// </summary>
        /// <param name="loggerFactory">The factory used to create loggers.</param>
        public EndpointMiddleware(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("Bytewizer.TinyCLR.Http");
        }

        /// <inheritdoc/>
        protected override void Invoke(HttpContext context, RequestDelegate next)
        {            
            var endpoint = context.GetEndpoint();
            if (endpoint?.RequestDelegate != null)
            {
                try
                {
                    //_logger.LogDebug($"Executing endpoint '{endpoint.DisplayName}'");
                    endpoint.RequestDelegate(context);
                }
                catch (Exception)
                {
                    throw;
                }
            }

            // TODO: Not sure if inline requests performance hit is worth this logger
            //_logger.LogDebug($"Executed endpoint '{endpoint.DisplayName}'");
            next(context);
        }
    }
}