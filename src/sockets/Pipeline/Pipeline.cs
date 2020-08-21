namespace Bytewizer.TinyCLR.Sockets
{
    public class Pipeline
    {
        private IPipelineFilter root;

        public Pipeline Register(IPipelineFilter filter)
        {
            if (root == null)
            {
                root = filter;
            }
            else
            {
                root.Register(filter);
            }

            return this;
        }

        public void Execute(IContext context)
        {
            root.Invoke(context);
        }
    }
}
