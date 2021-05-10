using System;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Diagnostics;

using Bytewizer.TinyCLR.Sockets.Channel;
using Bytewizer.TinyCLR.Sockets.Handlers;
using Bytewizer.TinyCLR.Sockets.Client;

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
        /// The port that the server is actively listening on.
        /// </summary>
        /// <remarks>
        /// You can use port <c>0</c> to let the OS assign a port. This method will then give you the assigned port.
        /// </remarks>
        public int ActivePort
        {
            get
            {
                if (_listenSocket == null)
                    return -1;

                return ((IPEndPoint)_listenSocket.LocalEndPoint).Port;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether <see cref="TcpListener"/> is actively listening for client connections.
        /// </summary>
        protected bool Active { get; private set; } = false;

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
            Debug.Assert(!Active, "Server is already started!");
            if (Active)
            {
                return true;
            }

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
                            Active = true;
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
                    Active = false;
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
            Debug.Assert(Active, "Service is not started!");
            if (!Active)
                return true;

            ThreadPool.Shutdown();

            lock (_lock)
            {
                Active = false;

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
                    Active = false;
                    throw;
                }

                return true;
            }
        }


        /// <summary>
        /// Accepts a pending connection request.
        /// </summary>
        /// <returns>A <see cref="Socket"/> used to send and receive data.</returns>
        public Socket AcceptSocket()
        {
            if (!Active)
            {
                throw new InvalidOperationException();
            }

            return _listenSocket.Accept();
        }

        /// <summary>
        /// Accepts a pending connection request.
        /// </summary>
        /// <returns>A <see cref="TcpClient"/> used to send and receive data.</returns>
        public TcpClient AcceptTcpClient()
        {
            if (!Active)
            {
                throw new InvalidOperationException();
            }

            Socket acceptedSocket = _listenSocket.Accept();

            return new TcpClient(acceptedSocket);
        }

        /// <summary>
        /// Accepted connection listening thread.
        /// </summary>
        internal virtual void AcceptConnection()
        {
        }

        /// <summary>
        /// Client has connected.
        /// </summary>
        protected virtual void OnConnected(SocketChannel channel)
        {
            Connected(this, channel);
        }

        /// <summary>
        /// Client has disconnected.
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