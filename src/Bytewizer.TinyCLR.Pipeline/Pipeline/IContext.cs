namespace Bytewizer.TinyCLR.Pipeline
{
    /// <summary>
    /// An interface for <see cref="IMiddleware"/>.
    /// </summary>
    public interface IContext 
    {
        /// <summary>
        /// Clears the context and connected socket channel.
        /// </summary>
        void Clear();
    }
}