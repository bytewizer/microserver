﻿using System;
using System.Reflection;
using System.Collections;
using System.Diagnostics;

using Bytewizer.TinyCLR.Http.Mvc.Middleware;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// A middleware for generating controller routes and mapping request to controller actions.
    /// </summary>
    public class ControllerMiddleware : Middleware
    {
        private readonly ControllerOptions _options;
        private readonly ControllerFactory _controllerFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerMiddleware"/> class.
        /// </summary>
        public ControllerMiddleware()
            : this(new ControllerOptions())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerMiddleware"/> class.
        /// </summary>
        /// <param name="options">The options for configuring the middleware.</param>
        public ControllerMiddleware(ControllerOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            _options = options;
            _controllerFactory = new ControllerFactory();
            _controllerFactory.Load();

            Debug.WriteLine("Controller Url(s) Loaded:");
            foreach (DictionaryEntry controller in _controllerFactory.Controllers)
            {
                Debug.WriteLine("  route url: /" + controller.Key.ToString());
            }
        }

        /// <inheritdoc/>
        protected override void Invoke(HttpContext context, RequestDelegate next)
        {
            string uri = context.Request.Path.TrimStart('/').TrimEnd('/').ToLower();

            if (_controllerFactory.TryMapping(uri, out ControllerMapping mapping))
            {
                string actionName = uri.Substring(uri.LastIndexOf('/') + 1);

                MethodInfo action = mapping.FindAction(actionName);
                _controllerFactory.Invoke(mapping.ControllerType, action, context);

                return;
            }

            next(context);
        }
    }
}