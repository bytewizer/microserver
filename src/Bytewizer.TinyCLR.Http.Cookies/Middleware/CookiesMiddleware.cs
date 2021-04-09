using Bytewizer.TinyCLR.Http.Cookies;
using Bytewizer.TinyCLR.Http.Features;
using Bytewizer.TinyCLR.Http.Header;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// A middleware for managing and parsing cookies for this context.
    /// </summary>
    public class CookiesMiddleware : Middleware
    {
        private readonly HttpCookieParser _cookieParser;

        /// <summary>
        /// Initializes a default instance of the <see cref="CookiesMiddleware"/> class.
        /// </summary>
        public CookiesMiddleware()
        {
            _cookieParser = new HttpCookieParser();
        }

        /// <inheritdoc/>
        protected override void Invoke(HttpContext context, RequestDelegate next)
        {
            var cookies = context.Request.Headers[HeaderNames.Cookie];
            if (cookies != null)
            {
                var cookiesFeature = new HttpCookiesFeature()
                {
                    Cookies = _cookieParser.Parse(cookies)
                };

                context.Features.Set(typeof(IHttpCookiesFeature), cookiesFeature);
            }

            next(context);

            var responseFeature = (HttpCookiesFeature)context.Features.Get(typeof(IHttpCookiesFeature));
            if (responseFeature?.ResponseCookies != null)
            {
                var responseCookies = responseFeature.ResponseCookies as CookieCollection;
                if (responseCookies.Count > 0)
                {
                    foreach (CookieValue cookie in responseCookies)
                    {
                        ((HeaderDictionary)context.Response.Headers).Add(HeaderNames.SetCookie, $"{cookie.Value}");
                    }
                }
            }
        }
    }
}