using System;

using Bytewizer.TinyCLR.Telnet.Features;


namespace Bytewizer.TinyCLR.Telnet
{
    /// <summary>
    /// Extension methods for <see cref="TelnetContext"/> related to routing.
    /// </summary>
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Extension method for getting the <see cref="Endpoint"/> for the current request.
        /// </summary>
        /// <param name="context">The <see cref="TelnetContext"/> context.</param>
        public static Endpoint GetEndpoint(this TelnetContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var endpointFeature = (SessionFeature)context.Features.Get(typeof(SessionFeature));

            return endpointFeature?.Endpoint;
        }

        /// <summary>
        /// Extension method for setting the <see cref="Endpoint"/> for the current request.
        /// </summary>
        /// <param name="context">The <see cref="TelnetContext"/> context.</param>
        /// <param name="endpoint">The <see cref="Endpoint"/>.</param>
        public static void SetEndpoint(this TelnetContext context, Endpoint endpoint)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var feature = (SessionFeature)context.Features.Get(typeof(SessionFeature));

            if (endpoint != null)
            {
                if (feature == null)
                {
                    feature = new SessionFeature();
                    context.Features.Set(typeof(SessionFeature),feature);
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