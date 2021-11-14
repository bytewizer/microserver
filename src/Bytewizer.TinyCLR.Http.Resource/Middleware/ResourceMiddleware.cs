using System;
using System.Resources;

using Bytewizer.TinyCLR.Http.Features;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// A middleware for generating resource object responses.
    /// </summary>
    public class ResourceMiddleware : Middleware
    {
        private readonly ResourceManager _resourceManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceMiddleware"/> class.
        /// </summary>
        /// <param name="resourceManager">The <see cref="ResourceManager"/> for configuring the middleware.</param>
        public ResourceMiddleware(ResourceManager resourceManager)
        {
            if (resourceManager == null)
            {
                throw new ArgumentNullException(nameof(resourceManager));
            }

            _resourceManager = resourceManager;
        }

        /// <inheritdoc/>
        protected override void Invoke(HttpContext context, RequestDelegate next)
        {
            var resourceManagerFeature = new ResourceManagerFeature
            {
                ResourceManager = _resourceManager
            };

            context.Features.Set(typeof(IResourceManagerFeature), resourceManagerFeature);

            next(context);
        }
    }
}