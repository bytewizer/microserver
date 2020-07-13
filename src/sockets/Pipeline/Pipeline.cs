namespace Bytewizer.Sockets
{
    public class Pipeline
    {
        private IMiddleware root;

        public Pipeline Register(IMiddleware filter)
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

        public void Execute(Context context)
        {
            root.Execute(context);
        }
    }
}
