namespace Bytewizer.TinyCLR.Pipeline
{
    /// <summary>
    /// An interface for <see cref="PipelineBuilder"/>.
    /// </summary>
    public interface IPipelineBuilder
    {
        /// <summary>
        /// Register a <see cref="FilterDelegate"/> filter to the pipeline. Filters are executed in the order they are added.
        /// </summary>
        /// <param name="filter">The <see cref="FilterDelegate"/> to include in the pipeline.</param>
        PipelineBuilder Register(FilterDelegate filter);

        /// <summary>
        /// Register a <see cref="IPipelineFilter"/> filter to the pipeline. Filters are executed in the order they are added.
        /// </summary>
        /// <param name="filter">The <see cref="FilterDelegate"/> to include in the pipeline.</param>
        PipelineBuilder Register(IPipelineFilter filter);
    }
}