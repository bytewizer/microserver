namespace Bytewizer.TinyCLR.Pipeline
{
    /// <summary>
    /// An interface for <see cref="ApplicationBuilder"/>.
    /// </summary>
    public interface IApplication
    {
        /// <summary>
        /// Add a <see cref="IMiddleware"/> delegate to the application pipeline. Middleware is executed in the order added.
        /// </summary>
        /// <param name="middleware">The <see cref="InvokeMiddleware"/> delegate to include in the application pipeline.</param>
        void Use(InvokeMiddlewareDelegate middleware);

        /// <summary>
        /// Add a <see cref="IMiddleware"/> to the application pipeline. Middleware is executed in the order added.
        /// </summary>
        /// <param name="middleware">The <see cref="IMiddleware"/> to include in the application pipeline.</param>
        void UseMiddleware(IMiddleware middleware);

        /// <summary>
        /// Invokes all middleware in the application pipeline.
        /// </summary>
        /// <param name="context">Encapsulates all socket information about an individual request.</param>
        void Invoke(IContext context);
    }
}