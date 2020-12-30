using System;
using System.Threading;
using System.Net.Sockets;

namespace Bytewizer.TinyCLR.Sockets.Listener
{
    /// <summary>
    /// Represents an implementation of the <see cref="SocketListener"/> which listens for remote clients.
    /// </summary>
    public class SocketListener
    {
        private Thread _thread;
        private Socket _listener;

        private readonly SocketListenerOptions _options;

        private readonly ManualResetEvent _acceptEvent = new ManualResetEvent(false);
        private readonly ManualResetEvent _startedEvent = new ManualResetEvent(false);

        private readonly object _lock = new object();

        /// <summary>
        /// An event that is raised when a client is connected.
        /// </summary>
        public event ConnectedHandler Connected = delegate { };

        /// <summary>
        /// An event that is raised when a client is disconnected.
        /// </summary>
        public event DisconnectedHandler Disconnected = delegate { };

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

                    _listener = new Socket(AddressFamily.InterNetwork, _options.SocketType, _options.ProtocolType);
                    _listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, _options.ReuseAddress);
                    _listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, _options.KeepAlive);
                    _listener.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, _options.NoDelay);

                    if (_options.ProtocolType == ProtocolType.Udp)
                    {
                        _listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, _options.Broadcast);
                    }

                    _listener.SendTimeout = _options.SendTimeout;
                    _listener.ReceiveTimeout = _options.ReceiveTimeout;

                    // Bind the socket to the local endpoint and listen for incoming connections
                    _listener.Bind(_options.EndPoint);
                    _listener.Listen(_options.MaxPendingConnections);

                    _thread = new Thread(() =>
                    {
                        if (_listener != null)
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

                    if (_listener != null)
                    {
                        // Dispose of listener
                        _listener.Close();
                        _listener = null;
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

        private void AcceptConnection()
        {
            int retry;

            // Signal the start method to continue
            _startedEvent.Set();

            while (IsActive)
            {
                retry = 0;

                try
                {
                    // Set the accept event to nonsignaled state
                    _acceptEvent.Reset();

                    // Waiting for a connection
                    var remoteSocket = _listener.Accept();

                    ThreadPool.QueueUserWorkItem(
                    new WaitCallback(delegate (object state)
                    {
                            // Signal the accept thread to continue
                            _acceptEvent.Set();

                            // Invoke the connected handler
                            Connected(this, remoteSocket);
                    }));

                    // Wait until a connection is made before continuing
                    _acceptEvent.WaitOne();
                }
                catch (SocketException ex)
                {
                    //The listen socket was closed
                    if (ex.IsIgnorableSocketException())
                    {
                        Disconnected(this, ex);
                        continue;
                    }

                    if (retry > _options.SocketRetry)
                        throw;

                    retry++;
                    continue;
                }
                catch (Exception ex)
                {
                    if (ex is ObjectDisposedException || ex is NullReferenceException)
                        break;

                    Disconnected(this, ex);

                    // Signal the accept thread to continue
                    _acceptEvent.Set();

                    // try again
                    continue;
                }
            }
        }
    }
}