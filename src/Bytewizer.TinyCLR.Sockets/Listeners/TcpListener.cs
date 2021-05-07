using System;
using System.Threading;
using System.Net.Sockets;
using System.Diagnostics;
using Bytewizer.TinyCLR.Sockets.Channel;

namespace Bytewizer.TinyCLR.Sockets.Listener
{
    /// <summary>
    /// Represents an implementation of the <see cref="SocketListener"/> which listens for remote TCP clients.
    /// </summary>
    public class TcpListener : SocketListener
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TcpListener"/> class.
        /// <param name="options">Factory used to create objects used in this library.</param>
        /// </summary>
        public TcpListener(SocketListenerOptions options)
           : base(options)
        {
        }

        private int _listeningSockets = 0;
        //private static readonly Random _random = new Random();

        /// <summary>
        /// Accepted connection listening thread
        /// </summary>
        internal override void  AcceptConnection()
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

                    // Immediately do somthing with request response if max connections is reached             
                    while (_listeningSockets >= _options.MaxConcurrentConnections)
                    {
                        Debug.WriteLine($"Maxium number of concurrent connections of {_listeningSockets} reached");
                        //Thread.Sleep(_random.Next(100));
                    }

                    // Waiting for a connection
                    var remoteSocket = _listenSocket.Accept();

                    var channel = new SocketChannel();
                    if (_options.IsTls)
                    {
                        channel.Assign(
                            remoteSocket,
                            _options.Certificate,
                            _options.SslProtocols);
                    }
                    else
                    {
                        channel.Assign(remoteSocket);
                    }

                    ThreadPool.QueueUserWorkItem(
                    new WaitCallback(delegate (object state)
                    {
                        // Signal the accept thread to continue
                        _acceptEvent.Set();

                        // Increment request count
                        Interlocked.Increment(ref _listeningSockets);

                        // Invoke the connected handler
                        OnConnected(channel);

                        // Decrease request count
                        Interlocked.Decrement(ref _listeningSockets);

                    }));

                    // Wait until a connection is made before continuing
                    _acceptEvent.WaitOne();
                }
                catch (SocketException ex)
                {
                    //The listen socket was closed
                    if (ex.IsIgnorableSocketException())
                    {
                        OnDisconnected(ex);
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

                    OnDisconnected(ex);

                    // Signal the accept thread to continue
                    _acceptEvent.Set();

                    // try again
                    continue;
                }
            }
        }
    }
}
