using System;
using System.Net.Sockets;

using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Pipeline;
using Bytewizer.TinyCLR.Http.Internal;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Represents an implementation of the <see cref="HttpServer"/> for creating web servers.
    /// </summary>
    public class HttpServer : SocketService, IServer
    {
        private readonly ILogger _logger;

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
            _logger = loggerFactory.CreateLogger("Bytewizer.TinyCLR.Http");
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

                // Check to make sure channel contains data
                if (context.Channel.InputStream.Length > 0)
                {   
                    // Invoke pipeline 
                    Application.Invoke(context);
                }

                // Close connection and clear context once pipeline is complete
                context.Clear();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, $"Unexpcted exception in {nameof(HttpServer)}.{nameof(ClientConnected)}");
                return;
            }
        }
    }
}