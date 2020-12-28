using System;

using Bytewizer.TinyCLR.Pipeline;
using Bytewizer.TinyCLR.Pipeline.Builder;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Represents a base implementation of the <see cref="Middleware"/> for creating an application request pipeline.
    /// </summary>
    public abstract class Middleware : IMiddleware
    {
        private IMiddleware next;

        /// <summary>
        /// Invokes a middleware in the application pipeline.
        /// </summary>
        /// <param name="context">The context for the request.</param>
        /// <param name="next">The next request handler to be executed.</param>
        protected abstract void Invoke(HttpContext context, RequestDelegate next);

        /// <inheritdoc/>
        public void Use(InlineMiddlewareDelegate middleware)
        {
            Use(new InlineMiddleware(middleware));
        }

        /// <inheritdoc/>
        public void Use(IMiddleware middleware)
        {
            if (middleware == null)
            {
                throw new ArgumentNullException(nameof(middleware));
            }

            if (next == null)
            {
                next = middleware;
            }
            else
            {
                next.Use(middleware);
            }
        }

        /// <inheritdoc/>
        void IApplication.Invoke(IContext context)
        {
            Invoke(context as HttpContext, ctx =>
                {
                    if (next != null)
                        next.Invoke(ctx);
                });
        }
    }
}





//using Bytewizer.TinyCLR.Pipeline;
//using Bytewizer.TinyCLR.Pipeline.Builder;

//namespace Bytewizer.TinyCLR.Http
//{
//    /// <summary>
//    /// Represents a base implementation of the <see cref="Middleware"/> for creating an application request pipeline.
//    /// </summary>
//    public abstract class Middleware : IMiddleware
//    {
//        private IMiddleware next;

//        /// <summary>
//        /// Request handling method.
//        /// </summary>
//        /// <param name="context">The <see cref="HttpContext"/> for the current request.</param>
//        /// <param name="next">The delegate representing the remaining middleware in the request pipeline.</param>
//        protected abstract void Invoke(HttpContext context, RequestDelegate next);

//        public void Use(InvokeMiddlewareDelegate middleware)
//        {
//            throw new System.NotImplementedException();
//        }

//        /// <inheritdoc/>
//        public void Use(MiddlewareDelegate middleware)
//        {
//            Use(middleware.Invoke());
//        }

//        public void Use(IMiddleware middleware)
//        {
//            if (next == null)
//            {
//                next = middleware;
//            }
//            else
//            {
//                next.Use(middleware);
//            }
//        }

//        void IApplication.Invoke(IContext context)
//        {
//            var httpContext = context as HttpContext;

//            Invoke(httpContext, ctx =>
//                {
//                    if (next != null)
//                        next.Invoke(ctx);
//                });
//        }
//    }
//}
