using Bytewizer.TinyCLR.Pipeline;

namespace Bytewizer.TinyCLR.Http
{
    public class ApplicationBuilder : IApplicationBuilder
    {
        private FilterDelegate[] filters = new FilterDelegate[0];

        /// <inheritdoc/>
        public ApplicationBuilder Use(FilterDelegate filter)
        {
            filters = filters.Append(filter);
            return this;
        }

        /// <inheritdoc/>
        public ApplicationBuilder Use(IPipelineFilter filter)
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
