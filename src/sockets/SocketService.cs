using System;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

using Bytewizer.TinyCLR.Sockets.Pipeline;
using Bytewizer.TinyCLR.Sockets.Listener;

namespace Bytewizer.TinyCLR.Sockets
{
    /// <summary>
    /// Represents a base implementation of <see cref="SocketService"/> which uses <see cref="SocketListener"/> for serving requests.
    /// </summary>
    public abstract class SocketService
    {
        private readonly ServerOptions _options;
        private readonly SocketListener _listener;
        private readonly IPipelineFilter _pipeline;

        /// <summary>
        /// The context of the request.
        /// </summary>
        protected IContext Context = new Context();

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketService"/> class with pre-configured default options.
        /// </summary>
        public SocketService()
            : this(_ => { }) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketService"/> class.
        /// </summary>
        /// <param name="pipeline">The request pipeline to invoke.</param>
        public SocketService(IPipelineBuilder pipeline)
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
        public SocketService(IPAddress address, int port, IPipelineBuilder pipeline)
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
        /// The request <see cref="PipelineFilter"/> to add to the pipeline.
        /// Filters are executed in the order they are added.
        /// </param>

        public SocketService(ServerOptionsDelegate configure, IPipelineFilter filter)
        {
            var options = new ServerOptions();
            
            if (filter != null)
            {
                options.Register(filter);
            }
            
            configure(options);

            _options = options;
            _listener = new SocketListener(_options.Listener);
            _listener.ClientConnected += ClientConnected;
            _pipeline = ((PipelineBuilder)_options.Pipeline).Build();
        }

        /// <summary>
        /// Start accepting incoming requests.
        /// </summary>
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

        /// <summary>
        /// Stop receiving incoming requests.
        /// </summary>
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

        private void ClientConnected(object sender, Socket socket)
        {
            var options = _options.Listener;
            try
            {
                using (socket)
                {
                    if (options.IsTls)
                    {
                        Context.Channel.Assign(socket, options.Certificate, options.SslProtocols);
                    }
                    else
                    {
                        Context.Channel.Assign(socket);
                    }

                    _pipeline.Invoke(Context);
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