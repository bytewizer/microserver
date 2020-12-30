using System;

using Bytewizer.TinyCLR.Http.Features;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Extension methods for <see cref="HttpContext"/> related to routing.
    /// </summary>
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Extension method for getting the <see cref="Endpoint"/> for the current request.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/> context.</param>
        public static Endpoint GetEndpoint(this HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var endpointFeature = (EndpointFeature)context.Features.Get(typeof(IEndpointFeature));

            return endpointFeature?.Endpoint;
        }

        /// <summary>
        /// Extension method for setting the <see cref="Endpoint"/> for the current request.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/> context.</param>
        /// <param name="endpoint">The <see cref="Endpoint"/>.</param>
        public static void SetEndpoint(this HttpContext context, Endpoint endpoint)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var feature = (EndpointFeature)context.Features.Get(typeof(IEndpointFeature));

            if (endpoint != null)
            {
                if (feature == null)
                {
                    feature = new EndpointFeature();
                    context.Features.Set(typeof(IEndpointFeature),feature);
                }

                feature.Endpoint = endpoint;
            }
            else
            {
                if (feature == null)
                {
                    // No endpoint to set and no feature on context. Do nothing
                    return;
                }

                feature.Endpoint = null;
            }
        }
    }
}