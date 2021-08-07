using System;

using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Pipeline;
using Bytewizer.TinyCLR.Http.Internal;
using Bytewizer.TinyCLR.Sockets.Channel;
using Bytewizer.TinyCLR.Sockets.Listener;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Represents an implementation of the <see cref="HttpServer"/> for creating web servers.
    /// </summary>
    public class HttpServer : SocketService, IServer
    {
        private readonly ILogger _logger;
        private readonly ContextPool _contextPool;
        private readonly HttpServerOptions _httpOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpServer"/> class.
        /// </summary>
        /// <param name="configure">The configuration options of <see cref="HttpServer"/> specific features.</param>
        public HttpServer(ServerOptionsDelegate configure)
            : this(NullLoggerFactory.Instance, new HttpServerOptions())
        {
            Initialize(configure);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketServer"/> class.
        /// </summary>
        /// <param name="loggerFactory">The factory used to create loggers.</param>
        /// <param name="configure">The configuration options of <see cref="SocketServer"/> specific features.</param>
        public HttpServer(ILoggerFactory loggerFactory, ServerOptionsDelegate configure)
            : this(loggerFactory, new HttpServerOptions())
        {
            Initialize(configure);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpServer"/> class.
        /// </summary>
        /// <param name="loggerFactory">The factory used to create loggers.</param>
        /// <param name="options">The options of <see cref="HttpServer"/> specific features.</param>
        public HttpServer(ILoggerFactory loggerFactory, ServerOptions options)
            : base(loggerFactory)
        {
            _options = options;
            _contextPool = new ContextPool();
            _logger = loggerFactory.CreateLogger("Bytewizer.TinyCLR.Http");
            _httpOptions = _options as HttpServerOptions;

            SetListener();
        }

        private void Initialize(ServerOptionsDelegate configure)
        {
            _options.Listen(80);
            _options.Pipeline(app =>
            {
                app.Use(new HttpMiddleware());
            });

            configure(_options);
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
            // Set channel error handler
            channel.ChannelError += ChannelError;

            try
            {
                // get context from context pool
                var context = _contextPool.GetContext(typeof(HttpContext)) as HttpContext;

                // assign channel
                context.Channel = channel;

                // set server header name
                context.Response.Headers[HeaderNames.Server] = _httpOptions.Name;

                // Check message size
                if (context.Channel.InputStream.Length < _options.Limits.MinMessageSize
                || context.Channel.InputStream.Length > _options.Limits.MaxMessageSize)
                {
                    _logger.InvalidMessageLimit(
                        context.Channel.InputStream.Length,
                        _options.Limits.MinMessageSize,
                        _options.Limits.MaxMessageSize
                        );
                }
                else
                {
                    // invoke pipeline 
                    _options.Application.Invoke(context);
                }

                // release context back to pool and close connection once pipeline is complete
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