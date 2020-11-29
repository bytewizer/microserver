using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.Pipeline;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Defines middleware that can be added to the application's request pipeline.
    /// </summary>
    public abstract class Middleware : IPipelineFilter
    {
        private IPipelineFilter next;

        /// <summary>
        /// Request handling method.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/> for the current request.</param>
        /// <param name="next">The delegate representing the remaining middleware in the request pipeline.</param>
        protected abstract void Invoke(HttpContext context, RequestDelegate next);

        public void Register(IPipelineFilter filter)
        {
            if (next == null)
            {
                next = filter;
            }
            else
            {
                next.Register(filter);
            }
        }

        void IPipelineFilter.Invoke(IContext context)
        {
            var httpContext = context as HttpContext;
            
            Invoke(httpContext, ctx => 
                {
                    if (next != null) 
                        next.Invoke(ctx);
                });
        }
    }
}
