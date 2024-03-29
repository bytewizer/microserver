﻿using System;

using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Pipeline;
using Bytewizer.TinyCLR.Hosting;
using Bytewizer.TinyCLR.Sockets.Channel;
using Bytewizer.TinyCLR.Sockets.Listener;

namespace Bytewizer.TinyCLR.Ftp
{
    /// <summary>
    /// Represents an implementation of the <see cref="FtpServer"/> for creating web servers.
    /// </summary>
    public class FtpServer : SocketService, IServerService
    {
        private readonly ILogger _logger;
        private readonly ContextPool _contextPool;
        private readonly FtpServerOptions _ftpOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="FtpServer"/> class.
        /// </summary>
        /// <param name="configure">The configuration options of <see cref="FtpServer"/> specific features.</param>
        public FtpServer(ServerOptionsDelegate configure)
            : this(NullLoggerFactory.Instance, new FtpServerOptions())
        {
            Initialize(configure);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FtpServer"/> class.
        /// </summary>
        /// <param name="loggerFactory">The factory used to create loggers.</param>
        /// <param name="configure">The configuration options of <see cref="FtpServer"/> specific features.</param>
        public FtpServer(ILoggerFactory loggerFactory, ServerOptionsDelegate configure)
            : this(loggerFactory, new FtpServerOptions())
        {
            Initialize(configure);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FtpServer"/> class.
        /// </summary>
        /// <param name="loggerFactory">The factory used to create loggers.</param>
        /// <param name="options">The options of <see cref="FtpServer"/> specific features.</param>
        public FtpServer(ILoggerFactory loggerFactory, ServerOptions options)
            : base(loggerFactory)
        {
            _options = options;
            _contextPool = new ContextPool();
            _logger = loggerFactory.CreateLogger("Bytewizer.TinyCLR.Ftp");
            _ftpOptions = _options as FtpServerOptions;

            SetListener();
        }

        private void Initialize(ServerOptionsDelegate configure)
        {
            _options.Listen(21);
            _options.Pipeline(app =>
            {
                app.Use(new FtpMiddleware(_logger, _ftpOptions));
            });

            configure(_options as FtpServerOptions);
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
            _logger.RemoteConnected(channel);

            // Set channel error handler
            channel.ChannelError += ChannelError;

            try
            {
                // get context from context pool
                var context = _contextPool.GetContext(typeof(FtpContext)) as FtpContext;

                // assign channel
                context.Channel = channel;

                try
                {
                    // Invoke pipeline
                    _options.Application.Invoke(context);
                }
                catch (Exception ex)
                {
                    _logger.UnhandledException(ex);

                    //var message = ex.Message.Replace("\r", " ").Replace("\n", "");
                    //context.Channel.Write(451, $"Exception thrown, message: {message}.");
                }

                _logger.RemoteClosed(channel);

                // release context back to pool and close connection once pipeline is complete
                _contextPool.Release(context);
            }
            catch (Exception ex)
            {
                _logger.UnhandledException(ex);
                return;
            }
        }

        ///// <summary>
        ///// Retrieves the public IP address of the server to use in passive mode.
        ///// </summary>
        ///// <param name="localIpAddress">Local IP address of the server</param>
        ///// <param name="serverOptions">FTP server options</param>
        ///// <param name="logger">Current logger instance</param>
        ///// <returns>Public IP address of the server.</returns>
        //        private IPAddress GetPublicAddress(IPAddress localIpAddress, FtpServerOptions serverOptions, ILogger logger)
        //        {
        //            var publicIpAddress = localIpAddress;

        //            if (!string.IsNullOrEmpty(serverOptions.PublicAddress)
        //                && !IPAddress.TryParse(serverOptions.PublicAddress, out publicIpAddress))
        //            {
        //                // Try resolve provided host name to IPv4 address
        //                try
        //                {
        //                    publicIpAddress =
        //                        Dns.GetHostEntry(serverOptions.PublicAddress.ToString())
        //;
        //                }
        //                catch (Exception error)
        //                {
        //                    //logger.WriteWarning(
        //                    //    TraceResources.CouldNotResolvePublicAddressFormat,
        //                    //    serverOptions.PublicAddress,
        //                    //    error.Message,
        //                    //    publicIpAddress = localIpAddress);
        //                }
        //            }

        //            return publicIpAddress;
        //        }


        //public string GetMyExternalIp()
        //{
        //    if (myExternalIp == null)
        //    {
        //        using (var client = new WebClient())
        //        {
        //            myExternalIp = client.DownloadString("https://api.ipify.org/");
        //        }
        //    }
        //    return myExternalIp;
        //}



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