namespace Bytewizer.TinyCLR.Pipeline.Builder
{
    /// <summary>
    /// An interface for <see cref="ApplicationBuilder"/>.
    /// </summary>
    public interface IApplication
    {
        /// <summary>
        /// Add a <see cref="IMiddleware"/> delegate to the application pipeline. Middleware is executed in the order added.
        /// </summary>
        /// <param name="middleware">The <see cref="InlineMiddleware"/> delegate to include in the application pipeline.</param>
        void Use(InlineMiddlewareDelegate middleware);

        /// <summary>
        /// Add a <see cref="IMiddleware"/> to the application pipeline. Middleware is executed in the order added.
        /// </summary>
        /// <param name="middleware">The <see cref="IMiddleware"/> to include in the application pipeline.</param>
        void Use(IMiddleware middleware);

        /// <summary>
        /// Invokes all middleware in the application pipeline.
        /// </summary>
        /// <param name="context">Encapsulates all socket information about an individual request.</param>
        void Invoke(IContext context);
    }
}