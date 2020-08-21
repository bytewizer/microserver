namespace Bytewizer.TinyCLR.Sockets
{
    public abstract class PipelineFilter : IPipelineFilter
    {
        private IPipelineFilter next;

        protected abstract void Invoke(IContext context, RequestDelegate next);

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
            Invoke(context, ctx => 
                {
                    if (next != null) 
                        next.Invoke(ctx);
                });
        }
    }
}
