using System;
using System.Collections;
using Bytewizer.TinyCLR.Pipeline;

namespace Bytewizer.TinyCLR.Http
{
    public class EndpointRoutingMiddleware : Middleware
    {
        public EndpointRoutingMiddleware()
        { 
        
        }

        protected override void Invoke(HttpContext context, RequestDelegate next)
        {
            context.Request.RouteValues = new RouteDictionary();

            RequestDelegate rd = (ctx) =>
            {
                ctx.Response.Write("Hello!");
            };

            ArrayList al = new ArrayList();
            var ep = new Endpoint(al, rd, "Hello");
            context.SetEndpoint(ep);

            next(context);
        }
    }
}