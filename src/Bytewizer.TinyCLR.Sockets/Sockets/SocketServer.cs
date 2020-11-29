using System;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

using Bytewizer.TinyCLR.Pipeline;

namespace Bytewizer.TinyCLR.Sockets
{
    /// <summary>
    /// Represents an implementation of the <see cref="SocketServer"/> for creating network servers.
    /// </summary>
    public class SocketServer : SocketService
    {
        /// <summary>
        /// Initializes a default instance of the <see cref="SocketServer"/> class.
        /// </summary>
        public SocketServer()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketServer"/> class.
        /// </summary>
        /// <param name="pipeline">The request pipeline to invoke.</param>
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

        /// <summary>
        /// A client has connected.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="socket">The socket for the connected end point.</param>
        protected override void ClientConnected(object sender, Socket socket)
        {
            try
            {
                var context = new SocketContext();

                // Assign socket
                if (Options.Listener.IsTls)
                {
                    context.Channel.Assign(
                        socket,
                        Options.Listener.Certificate,
                        Options.Listener.SslProtocols);
                }
                else
                {
                    context.Channel.Assign(socket);
                }

                // Invoke pipeline 
                Pipeline.Invoke(context);

                if (context != null)
                {
                    context = null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to accept incoming connection. Exception: { ex.Message } StackTrace: {ex.StackTrace}");
                return;
            }
        }
    }
}