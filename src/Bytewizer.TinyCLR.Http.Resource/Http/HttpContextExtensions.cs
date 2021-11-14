using System;
using System.Resources;

using Bytewizer.TinyCLR.Http.Features;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Convenience methods for <see cref="HttpContext"/>.
    /// </summary>
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Extension method for getting the <see cref="ResourceManager"/> for the current request.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/> context.</param>
        public static ResourceManager GetResourceManager(this HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var resourceManagerFeature = (ResourceManagerFeature)context.Features.Get(typeof(IResourceManagerFeature));
            if (resourceManagerFeature == null)
            {
                resourceManagerFeature = new ResourceManagerFeature();
            }

            return resourceManagerFeature.ResourceManager;
        }
    }
}