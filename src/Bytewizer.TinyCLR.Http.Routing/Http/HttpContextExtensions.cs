using System;

using Bytewizer.TinyCLR.Http.Routing;
using Bytewizer.TinyCLR.Http.Features;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Extension methods for <see cref="HttpContext"/> related to routing.
    /// </summary>
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Gets the <see cref="RouteData"/> associated with the provided <paramref name="httpContext"/>.
        /// </summary>
        /// <param name="httpContext">The <see cref="HttpContext"/> associated with the current request.</param>
        public static RouteData GetRouteData(this HttpContext httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            var routingFeature = (RoutingFeature)httpContext.Features.Get(typeof(IRoutingFeature));
            return routingFeature?.RouteData; //?? new RouteData(httpContext.Request.RouteValues);
        }

        //public static string[] GetRouteValue(this HttpContext httpContext, string key)
        //{
        //    if (httpContext == null)
        //    {
        //        throw new ArgumentNullException(nameof(httpContext));
        //    }

        //    if (key == null)
        //    {
        //        throw new ArgumentNullException(nameof(key));
        //    }



        //    rreturn httpContext.Features.Get<IRouteValuesFeature>()?.RouteValues[key];
        //}

        //public static EndPoint GetEndpoint(this HttpContext context)
        //{
        //    if (context == null)
        //    {
        //        throw new ArgumentNullException(nameof(context));
        //    }
        //    return context.Session.Socket.RemoteEndPoint;
        //}
    }
}