namespace Bytewizer.TinyCLR.Pipeline
{
    /// <summary>
    /// An interface for <see cref="PipelineFilter"/>.
    /// </summary>
    public interface IPipelineFilter
    {
        /// <summary>
        /// Register a <see cref="IPipelineFilter"/> filter to the pipeline. Filters are executed in the order they are added.
        /// </summary>
        /// <param name="filter">The <see cref="IPipelineFilter"/> to include in the pipeline.</param>
        void Register(IPipelineFilter filter);

        /// <summary>
        /// Invokes all filters in the pipeline.
        /// </summary>
        /// <param name="context">Encapsulates all socket information about an individual request.</param>
        void Invoke(IContext context);
    }
}