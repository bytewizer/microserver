using Bytewizer.TinyCLR.Sockets.Pipeline;

namespace Bytewizer.TinyCLR.Sockets
{
    /// <summary>
    /// Represents a base implementation of the <see cref="PipelineFilter"/> for creating pipeline filters.
    /// </summary>
    public abstract class PipelineFilter : IPipelineFilter
    {
        private IPipelineFilter next;

        /// <summary>
        /// Invokes a filter in the pipeline.
        /// </summary>
        /// <param name="context">The context for the request.</param>
        /// <param name="next">The next request handler to be executed.</param>
        protected abstract void Invoke(IContext context, RequestDelegate next);

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        void IPipelineFilter.Invoke(IContext context)
        {
            Invoke(context, ctx => 
                {
                    if (next != null) 
                        next.Invoke(ctx);
                });
        }
    }
}