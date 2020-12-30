using System;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Enables endpoint routing features for incoming requests.
    /// </summary>
    public class EndpointMiddleware : Middleware
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EndpointMiddleware"/> class.
        /// </summary>
        public EndpointMiddleware()
        {
        }

        /// <inheritdoc/>
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
                    throw;
                }
            }

            next(context);
        }
    }
}