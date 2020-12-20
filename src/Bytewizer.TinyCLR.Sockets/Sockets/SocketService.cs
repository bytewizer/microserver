using System;
using System.Net;
using System.Net.Sockets;

using Bytewizer.TinyCLR.Pipeline;
using Bytewizer.TinyCLR.Sockets.Listener;

namespace Bytewizer.TinyCLR.Sockets
{
    /// <summary>
    /// Represents a base implementation of <see cref="SocketService"/> which uses <see cref="SocketListener"/> for serving requests.
    /// </summary>
    public abstract class SocketService : IServer
    {       
        private readonly SocketListener _listener;

        /// <summary>
        /// Configuration options of server specific features.
        /// </summary>
        protected readonly ServerOptions Options;

        /// <summary>
        /// The socket pipeline used to invoke pipeline fiters.
        /// </summary>
        protected readonly IApplication Pipeline;

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketService"/> class with pre-configured default options.
        /// </summary>
        public SocketService()
            : this(_ => { }) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketService"/> class.
        /// </summary>
        /// <param name="pipeline">The request pipeline to invoke.</param>
        public SocketService(IApplicationBuilder pipeline)
            : this(options =>
            {
                options.Pipeline = pipeline;
            })
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketService"/> class.
        /// </summary>
        /// <param name="port">The port for receiving data.</param>
        public SocketService(int port)
            : this(options =>
            {
                options.Listen(IPAddress.Any, port);
            })
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketService"/> class.
        /// </summary>
        /// <param name="address">The ip address for receiving data.</param>
        /// <param name="port">The port for receiving data.</param>
        public SocketService(IPAddress address, int port)
            : this(options =>
            {
                options.Listen(address, port);
            })
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketService"/> class.
        /// </summary>
        /// <param name="address">The ip address for receiving data.</param>
        /// <param name="port">The port for receiving data.</param>
        /// <param name="pipeline">The request pipeline to invoke.</param>
        public SocketService(IPAddress address, int port, IApplicationBuilder pipeline)
            : this(options =>
            {
                options.Pipeline = pipeline;
                options.Listen(address, port);
            })
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketService"/> class.
        /// </summary>
        /// <param name="configure">The configuration options of <see cref="SocketService"/> specific features.</param>
        public SocketService(ServerOptionsDelegate configure)
            : this (configure, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketService"/> class.
        /// </summary>
        /// <param name="configure">The configuration options of <see cref="SocketService"/> specific features.</param>
        /// <param name="filter">
        /// The request <see cref="Middleware"/> to add to the pipeline.
        /// Filters are executed in the order they are added.
        /// </param>

        public SocketService(ServerOptionsDelegate configure, IMiddleware filter)
        {
            var options = new ServerOptions();
            
            if (filter != null)
            {
                options.Register(filter);
            }
            
            configure(options);

            Options = options;
            _listener = new SocketListener(Options.Listener);
            _listener.Connected += ClientConnected;
            Pipeline = ((ApplicationBuilder)Options.Pipeline).Build();
        }

        ///<inheritdoc/>
        public bool Start()
        {
            try
            {
                return _listener.Start();
            }
            catch
            {
                return false;
            }
        }

        ///<inheritdoc/>
        public bool Stop()
        {
            try
            {
                return _listener.Stop();
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// A client has connected.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="socket">The socket for the connected end point.</param>
        protected virtual void ClientConnected(object sender, Socket socket)
        { 
        }

    }
}