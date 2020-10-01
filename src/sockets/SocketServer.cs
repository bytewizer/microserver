using System.Net;

using Bytewizer.TinyCLR.Sockets.Pipeline;

namespace Bytewizer.TinyCLR.Sockets
{
    /// <summary>
    /// Represents an implementation of the <see cref="SocketServer"/> for creating network servers.
    /// </summary>
    public class SocketServer : SocketService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SocketServer"/> class.
        /// </summary>
        public SocketServer(IPipelineBuilder pipeline)
            : base(pipeline)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketServer"/> class.
        /// </summary>
        /// <param name="port">The port for receiving data.</param>
        public SocketServer(int port)
            : base(port)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketServer"/> class.
        /// </summary>
        /// <param name="configure">The configuration options of <see cref="SocketServer"/> specific features.</param>
        public SocketServer(ServerOptionsDelegate configure)
            : base(configure)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketServer"/> class.
        /// </summary>
        /// <param name="address">The ip address for receiving data.</param>
        /// <param name="port">The port for receiving data.</param>
        /// <param name="pipeline">The request pipeline to invoke.</param>
        public SocketServer(IPAddress address, int port, IPipelineBuilder pipeline)
            : base(address, port, pipeline)
        {
        }
    }
}