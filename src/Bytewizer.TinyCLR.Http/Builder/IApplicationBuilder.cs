using Bytewizer.TinyCLR.Pipeline;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// An interface for <see cref="ApplicationBuilder"/>.
    /// </summary>
    public interface IApplicationBuilder
    {
        /// <summary>
        /// Register a <see cref="FilterDelegate"/> filter to the pipeline. Filters are executed in the order they are added.
        /// </summary>
        /// <param name="filter">The <see cref="FilterDelegate"/> to include in the pipeline.</param>
        ApplicationBuilder Use(FilterDelegate filter);

        /// <summary>
        /// Register a <see cref="IPipelineFilter"/> filter to the pipeline. Filters are executed in the order they are added.
        /// </summary>
        /// <param name="filter">The <see cref="FilterDelegate"/> to include in the pipeline.</param>
        ApplicationBuilder Use(IPipelineFilter filter);
    }
}