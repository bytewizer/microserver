using System;

using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Pipeline;
using Bytewizer.TinyCLR.Http.Internal;
using Bytewizer.TinyCLR.Sockets.Channel;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Represents an implementation of the <see cref="HttpServer"/> for creating web servers.
    /// </summary>
    public class HttpServer : SocketService, IServer
    {
        private readonly ILogger _logger;
        private readonly ContextPool _ContextPool;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpServer"/> class.
        /// </summary>
        /// <param name="configure">The configuration options of <see cref="HttpServer"/> specific features.</param>
        public HttpServer(ServerOptionsDelegate configure)
            : this(NullLoggerFactory.Instance, new HttpMiddleware(), configure)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketServer"/> class.
        /// </summary>
        /// <param name="loggerFactory">The factory used to create loggers.</param>
        /// <param name="configure">The configuration options of <see cref="SocketServer"/> specific features.</param>
        public HttpServer(ILoggerFactory loggerFactory, ServerOptionsDelegate configure)
            : this(loggerFactory, new HttpMiddleware(), configure)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpServer"/> class.
        /// </summary>
        /// <param name="loggerFactory">The factory used to create loggers.</param>
        /// <param name="middleware">The <see cref="IMiddleware"/> to include in the application pipeline.</param>
        /// <param name="configure">The configuration options of <see cref="HttpServer"/> specific features.</param>
        public HttpServer(ILoggerFactory loggerFactory, IMiddleware middleware, ServerOptionsDelegate configure)
            : base(loggerFactory, middleware, configure)
        {
            _ContextPool = new ContextPool();
            _logger = loggerFactory.CreateLogger("Bytewizer.TinyCLR.Http");
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
                var context = _ContextPool.GetContext(typeof(HttpContext)) as HttpContext;
                
                // assign channel
                context.Channel = channel;

                // set server header name
                context.Response.Headers[HeaderNames.Server] = _options.Name;

                // check to make sure channel contains data
                if (context.Channel.InputStream.Length > 0)
                {   
                    // invoke pipeline 
                    _application.Invoke(context);
                }

                // release context back to pool and close connection once pipeline is complete
                _ContextPool.Release(context);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, $"Unexpcted exception in {nameof(HttpServer)}.{nameof(ClientConnected)}");
                return;
            }
        }
    }
}