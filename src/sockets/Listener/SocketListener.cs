using System;
using System.Threading;
using System.Net.Sockets;
using System.Diagnostics;

using Bytewizer.TinyCLR.Threading;

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
        private readonly ManualResetEvent _acceptSignal = new ManualResetEvent(false);
        private readonly ManualResetEvent _startedSignal = new ManualResetEvent(false);

        private static readonly object _lock = new object();

        /// <summary>
        /// An event that is raised when a client is connected.
        /// </summary>
        public event ConnectedHandler ClientConnected = delegate { };

        /// <summary>
        /// An event that is raised when a client is disconnected.
        /// </summary>
        public event DisconnectedHandler ClientDisconnected = delegate { };

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketListener"/> class.
        /// <param name="options">Factory used to create objects used in this library.</param>
        /// </summary>
        public SocketListener(SocketListenerOptions options)
        {
            _options = options;

            ThreadPool.SetMinThreads(_options.MaxThreads);
            ThreadPool.SetMaxThreads(_options.MaxThreads);
        }

        /// <summary>
        /// Gets if listener has been started.
        /// </summary>
        public bool IsActive { get; private set; } = false;

        /// <summary>
        /// Start listener.
        /// </summary>
        public bool Start()
        {
            // If service was already started the call has no effect
            if (IsActive)
                return true;
            
            lock (_lock)
            {

                try
                {
                    // Don't return until thread that calls Accept is ready to listen
                    _startedSignal.Reset();

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
                            AcceptConnections();
                        }
                    });
                    _thread.Priority = _options.ThreadPriority;
                    _thread.Start();

                    // Waits for thread that calls Accept to start
                    _startedSignal.WaitOne();

                    Debug.WriteLine($"Started socket listener bound on {_options.EndPoint}");
                }
                catch (Exception)
                {
                    IsActive = false;
                    return false;
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
            
            lock (_lock)
            {
                IsActive = false;

                try
                {
                    // Signal the accept thread to continue
                    _acceptSignal.Set();

                    if (_thread != null)
                    {
                        // Wait for thread to exit
                        _thread.Join(200);
                        _thread = null;
                    }

                    if (_listener != null)
                    {
                        // Dispose of listener
                        _listener.Close();
                        _listener = null;
                    }

                    Debug.WriteLine($"Stopped socket listener bound on {_options.EndPoint}");
                }
                catch (Exception)
                {
                    IsActive = false;
                    return false;
                }

                return true;
            }
        }

        private void AcceptConnections()
        {
            // Signal the start method to continue
            _startedSignal.Set();

            int retry;
            Socket remoteSocket;

            while (IsActive)
            {
                retry = 0;
                
                try
                {
                    // Set the event to nonsignaled state
                    _acceptSignal.Reset();

                    // Waiting for a connection
                    remoteSocket = _listener.Accept();
                    remoteSocket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, _options.NoDelay); // TODO: Do i need it in both places?

                    ThreadQueue.Instance.Enqueue(() => {
                        
                        // Signal the accept thread to continue
                        _acceptSignal.Set();

                        ClientConnected(this, remoteSocket);
                    });

                    // Wait until a connection is made before continuing
                    _acceptSignal.WaitOne();
                }
                catch (SocketException ex)
                {
                    //The listen socket was closed
                    if (ex.IsIgnorableSocketException())
                    {
                        ClientDisconnected(this, ex);
                        continue;
                    }

                    Debug.WriteLine($"Socket exception message: { ex.Message }");

                    if (retry > 5) throw;
                    retry++;
                    continue;
                }
                catch (Exception ex)
                {
                    if (ex is ObjectDisposedException || ex is NullReferenceException)
                        break;

                    ClientDisconnected(this, ex);

                    // Signal the accept thread to continue
                    _acceptSignal.Set();

                    // try again
                    continue;
                }
            }

            ThreadQueue.Instance.Dispose();
        }
    }
}