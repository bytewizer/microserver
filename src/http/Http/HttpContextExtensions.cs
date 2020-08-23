using System;
using System.Net;

namespace Bytewizer.TinyCLR.Http
{
    public static class HttpContextExtensions
    {
        public static RouteDictionary GetRouteData(this HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return context.Request.RouteValues;
        }

        public static string[] GetRouteValue(this HttpContext context, string key)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return context.Request.RouteValues[key];
        }

        public static EndPoint GetEndpoint(this HttpContext context) 
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            return context.Session.Socket.RemoteEndPoint;
        }
    }
}