using System;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.Pipeline;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Represents an implementation of the <see cref="HttpServer"/> for creating web servers.
    /// </summary>
    public class HttpServer : SocketService
    {
        /// <summary>
        /// Initializes a default instance of the <see cref="HttpServer"/> class.
        /// </summary>
        public HttpServer()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpServer"/> class.
        /// </summary>
        /// <param name="pipeline">The request pipeline to invoke.</param>
        public HttpServer(IPipelineBuilder pipeline)
            : base(pipeline)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpServer"/> class.
        /// </summary>
        /// <param name="port">The port for receiving data.</param>
        public HttpServer(int port)
            : base(port)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpServer"/> class.
        /// </summary>
        /// <param name="configure">The configuration options of <see cref="HttpServer"/> specific features.</param>
        public HttpServer(ServerOptionsDelegate configure)
            : base(configure, new HttpMiddleware())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpServer"/> class.
        /// </summary>
        /// <param name="address">The ip address for receiving data.</param>
        /// <param name="port">The port for receiving data.</param>
        /// <param name="pipeline">The request pipeline to invoke.</param>
        public HttpServer(IPAddress address, int port, IPipelineBuilder pipeline)
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
            //try
            //{
                // Creates a new HttpContext object per request.
                var context = new HttpContext();

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

                // Close connection and clear channel once pipeline is complete.
                context.Channel.Clear();

            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine($"Failed to accept incoming connection. Exception: { ex.Message } StackTrace: {ex.StackTrace}");
            //    return;
            //}
        }
    }
}