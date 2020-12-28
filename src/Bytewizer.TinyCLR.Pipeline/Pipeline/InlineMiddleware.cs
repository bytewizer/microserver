namespace Bytewizer.TinyCLR.Pipeline
{
    /// <summary>
    /// Represents an implementation of the <see cref="InlineMiddleware"/> for creating inline application pipeline request.
    /// </summary>
    public sealed class InlineMiddleware : Middleware
    {
        readonly InlineMiddlewareDelegate _middlewareDelegate;

        /// <summary>
        /// Initializes an instance of the <see cref="InlineMiddleware" /> class.
        /// </summary>
        public InlineMiddleware(InlineMiddlewareDelegate middleware)
        {
            _middlewareDelegate = middleware;
        }

        /// <summary>
        /// Invokes a middleware in the application pipeline.
        /// </summary>
        /// <param name="context">The context for the request.</param>
        /// <param name="next">The next request handler to be executed.</param>
        protected override void Invoke(IContext context, RequestDelegate next)
        {
            _middlewareDelegate.Invoke(context, next);
        }
    }
}