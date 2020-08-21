namespace Bytewizer.TinyCLR.Sockets
{
    public delegate IPipelineFilter FilterDelegate();

    public class PipelineBuilder : IPipelineBuilder
    {
        private FilterDelegate[] filters = new FilterDelegate[0];

        public PipelineBuilder Register(FilterDelegate filter)
        {
            filters = filters.Append(filter);
            return this;
        }

        public PipelineBuilder Register(IPipelineFilter filter)
        {
            filters = filters.Append(() => filter);
            return this;
        }

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