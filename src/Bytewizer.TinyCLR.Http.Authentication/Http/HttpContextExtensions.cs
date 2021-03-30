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
        /// Extension method for getting the current user name request.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/> context.</param>
        public static IUser GetCurrentUser(this HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var userFeature = (HttpAuthenticationFeature)context.Features.Get(typeof(IHttpAuthenticationFeature));

            return userFeature?.User;
        }
    }
}