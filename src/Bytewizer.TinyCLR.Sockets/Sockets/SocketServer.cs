﻿using System;
using System.Net.Sockets;

using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Pipeline;

namespace Bytewizer.TinyCLR.Sockets
{
    /// <summary>
    /// Represents an implementation of the <see cref="SocketServer"/> for creating network servers.
    /// </summary>
    public class SocketServer : SocketService, IServer
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketServer"/> class.
        /// </summary>
        /// <param name="configure">The configuration options of <see cref="SocketServer"/> specific features.</param>
        public SocketServer(ServerOptionsDelegate configure)
            : this(NullLoggerFactory.Instance, null, configure)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketServer"/> class.
        /// </summary>
        /// <param name="loggerFactory">The factory used to create loggers.</param>
        /// <param name="configure">The configuration options of <see cref="SocketServer"/> specific features.</param>
        public SocketServer(ILoggerFactory loggerFactory, ServerOptionsDelegate configure)
            : this(loggerFactory, null, configure)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketServer"/> class.
        /// </summary>
        /// <param name="loggerFactory">The factory used to create loggers.</param>
        /// <param name="middleware">The <see cref="IMiddleware"/> to include in the application pipeline.</param>
        /// <param name="configure">The configuration options of <see cref="SocketServer"/> specific features.</param>
        public SocketServer(ILoggerFactory loggerFactory, IMiddleware middleware, ServerOptionsDelegate configure)
            : base(loggerFactory, middleware, configure)
        {
            _logger = loggerFactory.CreateLogger("Bytewizer.TinyCLR.Sockets");
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

                // Check to make sure channel contains data
                if (context.Channel.InputStream.Length > 0)
                {
                    // Invoke pipeline 
                    Application.Invoke(context);
                }

                // Close connection and clear channel once pipeline is complete.
                context.Channel.Clear();

            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, $"Unexpcted exception in {nameof(SocketServer)}.{nameof(ClientConnected)}.");
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
            _logger.LogError(execption, $"Remote client disconnected connection");
        }
    }
}