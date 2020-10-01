using System.Net.Sockets;

namespace Bytewizer.TinyCLR.Sockets.Pipeline
{
    /// <summary>
    /// Represents an implementation of the <see cref="SocketPipeline"/>.
    /// </summary>
    public class SocketPipeline
    {
        private IPipelineFilter root;

        /// <summary>
        /// Register a <see cref="IPipelineFilter"/> filter to the pipeline. Filters are executed in the order they are added.
        /// </summary>
        /// <param name="filter">The filter to include in the pipeline.</param>
        public SocketPipeline Register(IPipelineFilter filter)
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

        /// <summary>
        ///  Invokes all filters in the pipeline.
        /// </summary>
        /// <param name="context">Encapsulates socket specific information about an individual request.</param>
        public void Execute(IContext context)
        {
            root.Invoke(context);
        }
    }
}
