using System;

using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Pipeline;
using Bytewizer.TinyCLR.Sockets.Channel;
using Bytewizer.TinyCLR.Sockets.Listener;

namespace Bytewizer.TinyCLR.Sockets
{
    /// <summary>
    /// Represents an implementation of the <see cref="SocketServer"/> for creating network servers.
    /// </summary>
    public class SocketServer : SocketService, IServer
    {
        private readonly ILogger _logger;
        private readonly ContextPool _contextPool = new ContextPool();

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketServer"/> class.
        /// </summary>
        /// <param name="configure">The configuration options of <see cref="SocketServer"/> specific features.</param>
        public SocketServer(ServerOptionsDelegate configure)
            : base(NullLoggerFactory.Instance)
        {
            configure(_options);

            SetListener();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketServer"/> class.
        /// </summary>
        /// <param name="loggerFactory">The factory used to create loggers.</param>
        /// <param name="configure">The configuration options of <see cref="SocketServer"/> specific features.</param>
        public SocketServer(ILoggerFactory loggerFactory, ServerOptionsDelegate configure)
            : base(loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("Bytewizer.TinyCLR.Sockets");
            configure(_options);

            SetListener();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketServer"/> class.
        /// </summary>
        /// <param name="loggerFactory">The factory used to create loggers.</param>
        /// <param name="options">The configuration options of <see cref="SocketServer"/> specific features.</param>
        public SocketServer(ILoggerFactory loggerFactory, ServerOptions options)
            : base(loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("Bytewizer.TinyCLR.Sockets");
            _options = options;

            SetListener();
        }

        /// <summary>
        /// Gets configuration options of socket specific features.
        /// </summary>
        public SocketListenerOptions ListenerOptions { get => _listener?.Options; }

        /// <summary>
        /// Gets port that the server is actively listening on.
        /// </summary>
        public int ActivePort { get => _listener.ActivePort; }

        /// <summary>
        /// A client has connected.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="channel">The socket channel for the connected end point.</param>
        protected override void ClientConnected(object sender, SocketChannel channel)
        {
            // Check to make sure channel contains data and valid message size
            if (channel.InputStream.Length < _options.Limits.MinMessageSize
                || channel.InputStream.Length > _options.Limits.MaxMessageSize)
            {
                _logger.InvalidMessageLimit(
                    channel.InputStream.Length,
                    _options.Limits.MinMessageSize,
                    _options.Limits.MaxMessageSize
                    );

                channel.Clear();
                return;
            }

            // Set channel error handler
            channel.ChannelError += ChannelError;

            try
            {
                // Get context from context pool
                var context = _contextPool.GetContext(typeof(SocketContext)) as SocketContext;

                // Assign channel
                context.Channel = channel;

                // Invoke pipeline 
                _options.Application.Invoke(context);

                // Release context back to pool and close connection once pipeline is complete
                _contextPool.Release(context);
            }
            catch (Exception ex)
            {
                _logger.UnhandledException(ex);
                return;
            }
        }

        /// <summary>
        /// A client has disconnected.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="execption">The socket <see cref="Exception"/> for the disconnected endpoint.</param>
        protected override void ClientDisconnected(object sender, Exception execption)
        {
            _logger.RemoteDisconnect(execption);
        }

        /// <summary>
        /// An internal channel error occured.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="execption">The <see cref="Exception"/> for the channel error.</param>
        private void ChannelError(object sender, Exception execption)
        {
            _logger.ChannelExecption(execption);
        }
    }
}