namespace Bytewizer.TinyCLR.Pipeline
{
    /// <summary>
    /// An interface for <see cref="ApplicationBuilder"/>.
    /// </summary>
    public interface IApplicationBuilder
    {
        /// <summary>
        /// Register a <see cref="IMiddleware"/> to the pipeline. Middleware are executed in the order added.
        /// </summary>
        /// <param name="middleware">The <see cref="IMiddleware"/> to include in the application pipeline.</param>
        IApplicationBuilder Register(IMiddleware middleware);

        /// <summary>
        /// Register a <see cref="IMiddleware"/> to the pipeline. Middleware are executed in the order added.
        /// </summary>
        /// <param name="middleware">The <see cref="InvokeMiddleware"/> delegate to include in the application pipeline.</param>
        IApplicationBuilder Register(InvokeMiddlewareDelegate middleware);
    }
}