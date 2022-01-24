using System;

using Bytewizer.TinyCLR.Http.Cookies;
using Bytewizer.TinyCLR.Http.Features;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Convenience methods for <see cref="HttpContext"/>.
    /// </summary>
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Extension method for getting the <see cref="CookieCollection"/> for the current request.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/> context.</param>
        public static ICookieCollection GetCookies(this HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var cookiesFeature = (HttpCookiesFeature)context.Features.Get(typeof(IHttpCookiesFeature));
            if (cookiesFeature == null)
            {
                cookiesFeature = new HttpCookiesFeature
                {
                    Cookies = new CookieCollection()
                };
                context.Features.Set(typeof(IHttpCookiesFeature), cookiesFeature);
            }

            return cookiesFeature.Cookies;
        }

        /// <summary>
        /// Extension method for getting the <see cref="CookieCollection"/> used to manage cookies for this response.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/> context.</param>
        public static IResponseCookies GetResponseCookies(this HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var cookiesFeature = (HttpCookiesFeature)context.Features.Get(typeof(IHttpCookiesFeature));
            if (cookiesFeature.ResponseCookies == null)
            {
                cookiesFeature = new HttpCookiesFeature
                {
                    ResponseCookies = new CookieCollection()
                };
                context.Features.Set(typeof(IHttpCookiesFeature), cookiesFeature);
            }

            return cookiesFeature.ResponseCookies;
        }
    }
}