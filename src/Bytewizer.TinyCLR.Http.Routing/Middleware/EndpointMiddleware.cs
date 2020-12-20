using System;

namespace Bytewizer.TinyCLR.Http
{
    internal sealed class EndpointMiddleware : Middleware
    {
        public EndpointMiddleware()
        {
        }

        protected override void Invoke(HttpContext context, RequestDelegate next)
        {            
            var endpoint = context.GetEndpoint();
            if (endpoint?.RequestDelegate != null)
            {
                try
                {
                    endpoint.RequestDelegate(context);
                }
                catch (Exception)
                {
                }
            }

            next(context);
        }
    }
}