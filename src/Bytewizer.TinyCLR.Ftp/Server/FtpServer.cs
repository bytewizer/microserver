using System;
using System.Net;

using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Pipeline;
using Bytewizer.TinyCLR.Ftp.Internal;
using Bytewizer.TinyCLR.Sockets.Channel;

namespace Bytewizer.TinyCLR.Ftp
{
    /// <summary>
    /// Represents an implementation of the <see cref="FtpServer"/> for creating web servers.
    /// </summary>
    public class FtpServer : SocketService, IServer
    {
        private readonly ILogger _logger;
        private readonly ContextPool _contextPool;

        /// <summary>
        /// Initializes a new instance of the <see cref="FtpServer"/> class.
        /// </summary>
        /// <param name="configure">The configuration options of <see cref="FtpServer"/> specific features.</param>
        public FtpServer(ServerOptionsDelegate configure)
            : this(NullLoggerFactory.Instance, new ServerOptions())
        {
            _options.Pipeline(app =>
            {
                app.Use(new FtpMiddleware());
            });

            configure(_options);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FtpServer"/> class.
        /// </summary>
        /// <param name="loggerFactory">The factory used to create loggers.</param>
        /// <param name="options">The options of <see cref="FtpServer"/> specific features.</param>
        public FtpServer(ILoggerFactory loggerFactory, ServerOptions options)
            : base(loggerFactory, options)
        {
            _contextPool = new ContextPool();
            _logger = loggerFactory.CreateLogger("Bytewizer.TinyCLR.Http");

            _options.Listen(21);
        }

        /// <summary>
        /// A client has connected.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="channel">The socket channel for the connected end point.</param>
        protected override void ClientConnected(object sender, SocketChannel channel)
        {
            try
            {
                // get context from context pool
                var context = _contextPool.GetContext(typeof(FtpContext)) as FtpContext;
                
                // assign channel
                context.Channel = channel;

                // check to make sure channel contains data
                //if (context.Channel.InputStream.Length > 0)
                //{   
                    // invoke pipeline 
                    _options.Application.Invoke(context);
                //}

                // release context back to pool and close connection once pipeline is complete
                _contextPool.Release(context);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, $"Unexpcted exception in {nameof(FtpServer)}.{nameof(ClientConnected)}");
                return;
            }
        }
    }
}