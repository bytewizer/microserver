using System;

using Bytewizer.TinyCLR.Pipeline.Builder;

namespace Bytewizer.TinyCLR.Pipeline
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
        protected abstract void Invoke(IContext context, RequestDelegate next);

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
            Invoke(context, ctx => 
                {
                    if (next != null) 
                        next.Invoke(ctx);
                });
        }
    }
}