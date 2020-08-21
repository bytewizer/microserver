using System;

namespace Bytewizer.TinyCLR.Http
{
    public static class HttpContextExtensions
    {
        public static RouteDictionary GetRouteData(this HttpContext httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            return httpContext.Request.RouteValues;
        }

        public static string[] GetRouteValue(this HttpContext httpContext, string key)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            return httpContext.Request.RouteValues[key];
        }
    }
}