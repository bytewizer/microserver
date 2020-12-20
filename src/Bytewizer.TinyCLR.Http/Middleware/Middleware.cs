using Bytewizer.TinyCLR.Pipeline;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Defines middleware that can be added to the application's request pipeline.
    /// </summary>
    public abstract class Middleware : IMiddleware
    {
        private IMiddleware next;

        /// <summary>
        /// Request handling method.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/> for the current request.</param>
        /// <param name="next">The delegate representing the remaining middleware in the request pipeline.</param>
        protected abstract void Invoke(HttpContext context, RequestDelegate next);

        public void Use(InvokeMiddlewareDelegate middleware)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public void Use(MiddlewareDelegate middleware)
        {
            UseMiddleware(middleware.Invoke());
        }

        public void UseMiddleware(IMiddleware middleware)
        {
            if (next == null)
            {
                next = middleware;
            }
            else
            {
                next.UseMiddleware(middleware);
            }
        }

        void IApplication.Invoke(IContext context)
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
