namespace Bytewizer.Sockets
{
    public abstract class Middleware : IMiddleware
    {
        private IMiddleware next;

        protected abstract void Execute(Context context, RequestDelegate next);

        public void Register(IMiddleware filter)
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

        void IMiddleware.Execute(Context context)
        {
            Execute(context, ctx => 
                {
                    if (next != null) 
                        next.Execute(ctx);
                });
        }
    }
}
