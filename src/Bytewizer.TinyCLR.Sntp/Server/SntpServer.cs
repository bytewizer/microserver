using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.Logging;

namespace Bytewizer.TinyCLR.Sntp
{
    /// <summary>
    /// Represents an implementation of the <see cref="SntpServer"/> for creating web servers.
    /// </summary>
    public class SntpServer :  IServer
    {
        private readonly SocketServer _sntpServer;
        private readonly ILoggerFactory _loggerFactory = NullLoggerFactory.Instance;
        private readonly SntpServerOptions _sntpOptions = new SntpServerOptions();

        /// <summary>
        /// Initializes a new instance of the <see cref="SntpServer"/> class.
        /// </summary>
        public SntpServer()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SntpServer"/> class.
        /// </summary>
        /// <param name="configure">The configuration options of <see cref="SntpServer"/> specific features.</param>
        public SntpServer(ServerOptionsDelegate configure)
        {
            Initialize();

            configure(_sntpOptions);

            _sntpServer = new SocketServer(NullLoggerFactory.Instance, _sntpOptions);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SntpServer"/> class.
        /// </summary>
        /// <param name="loggerFactory">The factory used to create loggers.</param>
        /// <param name="configure">The configuration options of <see cref="SntpServer"/> specific features.</param>
        public SntpServer(ILoggerFactory loggerFactory, ServerOptionsDelegate configure)
        {
            _loggerFactory = loggerFactory;
            
            Initialize();

            configure(_sntpOptions);

            _sntpServer = new SocketServer(_loggerFactory, _sntpOptions);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SntpServer"/> class.
        /// </summary>
        /// <param name="loggerFactory">The factory used to create loggers.</param>
        /// <param name="options">The options of <see cref="SntpServer"/> specific features.</param>
        public SntpServer(ILoggerFactory loggerFactory, ServerOptions options)
        {
            _sntpServer = new SocketServer(loggerFactory, options);
        }

        private void Initialize()
        {
            // RFC limits message size to a 48 byte minimum and 68 byte maximum
            _sntpOptions.Limits.MinMessageSize = 48;
            _sntpOptions.Limits.MaxMessageSize = 68;

            // Set listening port 
            _sntpOptions.Listen(123, listener =>
            {
                listener.UseUdp();
                listener.Broadcast = true;
            });

            // Set pipleline options
            _sntpOptions.Pipeline(app =>
            {
                app.UseSntp(_loggerFactory, _sntpOptions);
            });
        }

        /// <inheritdoc/>
        public bool Start()
        {
            return _sntpServer.Start();
        }

        /// <inheritdoc/>
        public bool Stop()
        {
            return _sntpServer.Stop();
        }   
    }
}





///// <summary>
///// A client has connected.
///// </summary>
///// <param name="sender">The source of the event.</param>
///// <param name="channel">The socket channel for the connected end point.</param>
//protected void ClientConnected(object sender, SocketChannel channel)
//{
//    if (channel.InputStream.Length < _sntpOptions.Limits.MinMessageSize
//        || channel.InputStream.Length > _sntpOptions.Limits.MaxMessageSize)
//    {
//        channel.Clear();
//        return;
//    }

//    // Set channel error handler
//    channel.ChannelError += ChannelError;

//    try
//    {
//        // get context from context pool
//        var context = _contextPool.GetContext(typeof(SocketContext)) as SocketContext;

//        // assign channel
//        context.Channel = channel;

//        // Check to make sure channel contains data
//        if (context.Channel.InputStream.Length > 0)
//        {
//            // Invoke pipeline 
//            _sntpServer.Application.Invoke(context);
//        }

//        // release context back to pool and close connection once pipeline is complete
//        //_contextPool.Release(context);
//    }
//    catch (Exception ex)
//    {
//        _logger.LogCritical(ex, $"Unexpcted exception in {nameof(SntpServer)}.{nameof(ClientConnected)}");
//        return;
//    }
//}

///// <summary>
///// An internal channel error occured.
///// </summary>
///// <param name="sender">The source of the event.</param>
///// <param name="execption">The <see cref="Exception"/> for the channel error.</param>
//private void ChannelError(object sender, Exception execption)
//{
//    _logger.LogError(execption, $"Unexpcted channel exception in {nameof(SocketServer)}");
//}


//private readonly ILogger _logger;
//private readonly ContextPool _contextPool;
//private readonly SntpServerOptions _sntpOptions;

///// <summary>
///// Initializes a new instance of the <see cref="SntpServer"/> class.
///// </summary>
///// <param name="configure">The configuration options of <see cref="SntpServer"/> specific features.</param>
//public SntpServer(ServerOptionsDelegate configure)
//            : this(NullLoggerFactory.Instance, new SntpServerOptions())
//        {
//    Initialize(configure);
//}

///// <summary>
///// Initializes a new instance of the <see cref="SntpServer"/> class.
///// </summary>
///// <param name="loggerFactory">The factory used to create loggers.</param>
///// <param name="configure">The configuration options of <see cref="SntpServer"/> specific features.</param>
//public SntpServer(ILoggerFactory loggerFactory, ServerOptionsDelegate configure)
//            : this(loggerFactory, new SntpServerOptions())
//        {
//    Initialize(configure);
//}

///// <summary>
///// Initializes a new instance of the <see cref="SntpServer"/> class.
///// </summary>
///// <param name="loggerFactory">The factory used to create loggers.</param>
///// <param name="options">The options of <see cref="SntpServer"/> specific features.</param>
//private SntpServer(ILoggerFactory loggerFactory, ServerOptions options)
//            : base(loggerFactory, options)
//        {
//    _logger = loggerFactory.CreateLogger("Bytewizer.TinyCLR.Sntp");

//    _contextPool = new ContextPool();
//    _sntpOptions = _options as SntpServerOptions;

//    SetListener();
//}

///// <summary>
///// Initializes a new instance of the <see cref="SntpServer"/> class specific features.
///// </summary>
///// <param name="configure">The configuration options of <see cref="SntpServer"/> specific features.</param>
//private void Initialize(ServerOptionsDelegate configure)
//{
//    _options.Pipeline(app =>
//    {
//        app.Use(new SntpMiddleware(_logger, _sntpOptions));
//    });
//    _options.Listen(123, listener =>
//    {
//        listener.UseUdp();
//    });

//    configure(_options as SntpServerOptions);

//    //SetListener();
//}