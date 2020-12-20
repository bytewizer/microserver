namespace Bytewizer.TinyCLR.Pipeline
{
    internal sealed class InvokeMiddleware : Middleware
    {
        readonly InvokeMiddlewareDelegate _middlewareDelegate;

        public InvokeMiddleware(InvokeMiddlewareDelegate middleware)
        {
            _middlewareDelegate = middleware;
        }

        protected override void Invoke(IContext context, RequestDelegate next)
        {
            _middlewareDelegate.Invoke(context, next);
        }
    }
}