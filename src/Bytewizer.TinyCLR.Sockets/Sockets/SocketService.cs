using System;
using System.Diagnostics;
using System.Net.Sockets;

using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Sockets.Channel;
using Bytewizer.TinyCLR.Sockets.Listener;

namespace Bytewizer.TinyCLR.Sockets
{
    /// <summary>
    /// Represents a base implementation of <see cref="SocketService"/> which uses <see cref="SocketListener"/> for serving requests.
    /// </summary>
    public abstract class SocketService : IServer
    {
        /// <summary>
        /// The <see cref="SocketListener"/> which listens for remote clients.
        /// </summary>
        private readonly SocketListener _listener;

        /// <summary>
        /// The logger used to write to.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// The configuration options of server specific features.
        /// </summary>
        protected readonly IServerOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketService"/> class.
        /// </summary>
        /// <param name="loggerFactory">The factory used to create loggers.</param>
        /// <param name="options">The configuration options of <see cref="SocketService"/> specific features.</param>
        public SocketService(ILoggerFactory loggerFactory, ServerOptions options)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _logger = loggerFactory.CreateLogger("Bytewizer.TinyCLR.Sockets");

            switch (options.Listener.ProtocolType)
            {
                case ProtocolType.Tcp:
                    _listener = new TcpListener(options.Listener);
                    break;
                case ProtocolType.Udp:
                    _listener = new UdpListener(options.Listener);
                    break;
                default:
                    throw new NotSupportedException();
            }
            
            _listener.Connected += ClientConnected;
            _listener.Disconnected += ClientDisconnected;

            _options = options;
        }

        ///<inheritdoc/>
        public bool Start()
        {
            try
            {
                var status = _listener.Start();
                WriteMessage($"Started socket listener bound to port {_listener.ActivePort}");

                return status;
            }
            catch (Exception ex)
            {
                WriteExecption(ex, $"Error starting listerner bound to port {_listener.ActivePort}");
                return false;
            }
        }

        ///<inheritdoc/>
        public bool Stop()
        {
            try
            {
                var status = _listener.Stop();
                WriteMessage($"Stopping socket listener bound to port {_listener.ActivePort}");

                return status;
            }
            catch (Exception ex)
            {
                WriteExecption(ex, $"Error stopping listener bound to port {_listener.ActivePort}");
                return false;
            }
        }

        private void WriteMessage(string message)
        {
            if (_logger.GetType() == typeof(NullLogger))
            {
                Debug.WriteLine(message);
            }
            else
            {
                _logger.LogInformation(message);
            }
        }

        private void WriteExecption(Exception execption, string message)
        {
            if (_logger.GetType() == typeof(NullLogger))
            {
                Debug.WriteLine($"{message} : {execption}");
            }
            else
            {
                _logger.LogError(execption, message);
            }
        }

        /// <summary>
        /// A client has connected.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="channel">The socket channel for the connected endpoint.</param>
        protected virtual void ClientConnected(object sender, SocketChannel channel)
        {
        }

        /// <summary>
        /// A client has disconnected.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="execption">The socket <see cref="Exception"/> for the disconnected endpoint.</param>
        protected virtual void ClientDisconnected(object sender, Exception execption)
        {
        }
    }
}