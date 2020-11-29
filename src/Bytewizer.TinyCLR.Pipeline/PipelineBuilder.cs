namespace Bytewizer.TinyCLR.Pipeline
{
    /// <summary>
    /// Represents an implementation of the <see cref="PipelineBuilder"/> for creating pipelines.
    /// </summary>
    public class PipelineBuilder : IPipelineBuilder
    {
        private FilterDelegate[] filters = new FilterDelegate[0];

        /// <inheritdoc/>
        public PipelineBuilder Register(FilterDelegate filter)
        {
            filters = filters.Append(filter);
            return this;
        }

        /// <inheritdoc/>
        public PipelineBuilder Register(IPipelineFilter filter)
        {
            filters = filters.Append(() => filter);
            return this;
        }

        /// <summary>
        /// Builds pipeline from registered filters. 
        /// </summary>
        public IPipelineFilter Build()
        {
            var root = filters[0].Invoke();

            for (int i = 1; i < filters.Length; i++)
            {
                root.Register(filters[i].Invoke());
            }

            return root;
        }
    }
}