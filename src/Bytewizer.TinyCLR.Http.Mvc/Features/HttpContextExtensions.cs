//using System;

//using Bytewizer.TinyCLR.Http.Features;

//namespace Bytewizer.TinyCLR.Http
//{
//    public static class HttpContextExtensions
//    {
//        public static Endpoint GetEndpoint(this HttpContext context)
//        {
//            if (context == null)
//            {
//                throw new ArgumentNullException(nameof(context));
//            }

//            var ctx = context.Features.Get(typeof(IEndpointFeature)) as EndpointFeature;

//            return ctx?.Endpoint;

//        }

//        public static void SetEndpoint(this HttpContext context, Endpoint endpoint)
//        {
//            if (context == null)
//            {
//                throw new ArgumentNullException(nameof(context));
//            }

//            var feature = new EndpointFeature
//            {
//                Endpoint = endpoint
//            };

//            context.Features.Set(typeof(IEndpointFeature), feature);
//        }
//    }
//}

