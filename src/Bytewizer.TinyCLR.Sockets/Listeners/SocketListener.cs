using System;
using System.Threading;
using System.Net.Sockets;
using System.Diagnostics;

using Bytewizer.TinyCLR.Sockets.Channel;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace Bytewizer.TinyCLR.Sockets.Listener
{
    /// <summary>
    /// Represents an implementation of the <see cref="SocketListener"/> which listens for remote clients.
    /// </summary>
    public abstract class SocketListener
    {
        internal Thread _thread;
        internal Socket _listenSocket;

        internal readonly SocketListenerOptions _options;

        internal readonly ManualResetEvent _acceptEvent = new ManualResetEvent(false);
        internal readonly ManualResetEvent _startedEvent = new ManualResetEvent(false);

        private readonly object _lock = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketListener"/> class.
        /// <param name="options">Factory used to create objects used in this library.</param>
        /// </summary>
        public SocketListener(SocketListenerOptions options)
        {
            _options = options;
        }

        /// <summary>
        /// Gets if listener has been started.
        /// </summary>
        public bool IsActive { get; private set; } = false;

        /// <summary>
        /// Gets the <see cref="SocketListenerOptions"/> used to configure <see cref="SocketListener"/>.
        /// </summary>
        public SocketListenerOptions Options => _options;

        /// <summary>
        /// Start listener.
        /// </summary>
        public bool Start()
        {
            // If service was already started the call has no effect
            Debug.Assert(!IsActive, "Server is already started!");
            if (IsActive)
                return true;

            ThreadPool.SetMinThreads(_options.MaxThreads);
            ThreadPool.SetMaxThreads(_options.MaxThreads);

            lock (_lock)
            {
                try
                {
                    // Don't return until thread that calls Accept is ready to listen
                    _startedEvent.Reset();

                    _listenSocket = new Socket(AddressFamily.InterNetwork, _options.SocketType, _options.ProtocolType);
                    _listenSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, _options.ReuseAddress);
                    _listenSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, _options.KeepAlive);

                    _listenSocket.SendTimeout = _options.SendTimeout;
                    _listenSocket.ReceiveTimeout = _options.ReceiveTimeout;

                    if (_options.ProtocolType == ProtocolType.Tcp)
                    {
                        _listenSocket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, _options.NoDelay);
                    }

                    if (_options.ProtocolType == ProtocolType.Udp)
                    {
                        _listenSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, _options.Broadcast);
                    }

                    // Bind the socket to the local endpoint and listen for incoming connections
                    _listenSocket.Bind(_options.EndPoint);

                    if (_options.ProtocolType == ProtocolType.Tcp)
                    {
                        _listenSocket.Listen(_options.MaxPendingConnections);
                    }

                    _thread = new Thread(() =>
                    {
                        if (_listenSocket != null)
                        {
                            IsActive = true;
                            AcceptConnection();
                        }
                    });
                    _thread.Priority = _options.ThreadPriority;
                    _thread.Start();

                    // Waits for thread that calls Accept to start
                    _startedEvent.WaitOne();
                }
                catch
                {
                    IsActive = false;
                    throw;
                }

                return true;
            }
        }

        /// <summary>
        /// Stop the active listener.
        /// </summary>
        public bool Stop()
        {
            // If service was already started the call has no effect
            Debug.Assert(IsActive, "Service is not started!");
            if (!IsActive)
                return true;

            ThreadPool.Shutdown();

            lock (_lock)
            {
                IsActive = false;

                try
                {
                    // Signal the accept thread to continue
                    _acceptEvent.Set();

                    if (_thread != null)
                    {
                        // Wait for thread to exit
                        _thread.Join(1000);
                        _thread = null;
                    }

                    if (_listenSocket != null)
                    {
                        // Dispose of listener
                        _listenSocket.Close();
                        _listenSocket = null;
                    }
                }
                catch
                {
                    IsActive = false;
                    throw;
                }

                return true;
            }
        }


        /// <summary>
        /// Accepted connection listening thread
        /// </summary>
        internal virtual void AcceptConnection()
        {
        }

        /// <summary>
        /// A client has connected.
        /// </summary>
        protected virtual void OnConnected(SocketChannel channel)
        {
            Connected(this, channel);
        }

        /// <summary>
        /// A client has disconnected.
        /// </summary>
        protected virtual void OnDisconnected(Exception exception)
        {
            Disconnected(this, exception);
        }

        /// <summary>
        /// An event that is raised when a client is connected.
        /// </summary>
        public event ConnectedHandler Connected = delegate { };

        /// <summary>
        /// An event that is raised when a client is disconnected.
        /// </summary>
        public event DisconnectedHandler Disconnected = delegate { };
    }
}