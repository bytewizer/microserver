using System;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Diagnostics;

#if NanoCLR
using Bytewizer.NanoCLR.Sockets.Channel;
using Bytewizer.NanoCLR.Sockets.Extensions;

namespace Bytewizer.NanoCLR.Sockets.Listener
#else
using Bytewizer.TinyCLR.Sockets.Channel;
using Bytewizer.TinyCLR.Sockets.Extensions;

namespace Bytewizer.TinyCLR.Sockets.Listener
#endif
{
    /// <summary>
    /// Represents an implementation of the <see cref="SocketListener"/> which listens for remote UDP clients.
    /// </summary>
    public class UdpListener : SocketListener
    {
        private int _listeningSockets = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="UdpListener"/> class.
        /// <param name="options">Factory used to create objects used in this library.</param>
        /// </summary>
        public UdpListener(SocketListenerOptions options)
           : base(options)
        {
        }

        /// <summary>
        /// Accepted connection listening thread
        /// </summary>
        internal override void AcceptConnection()
        {
            int retry;

            // Signal the start method to continue
            _startedEvent.Set();

            while (Active)
            {
                retry = 0;

                try
                {
                    // Set the accept event to nonsignaled state
                    _acceptEvent.Reset();

                    while (_listenSocket.Poll(_options.PollTimeout, SelectMode.SelectRead))
                    {
                        if (_listenSocket.Available <= 0)
                        {
                            return;
                        }

                        // Delay request response if max connections is reached             
                        while (_listeningSockets >= _options.MaxConcurrentConnections)
                        {
                            Debug.WriteLine($"Maxium number of concurrent connections of {_listeningSockets} reached.");
                            Thread.Sleep(100);
                        }

                        // Allow remote connections from all endponts
                        var remoteEndPoint = new IPEndPoint(IPAddress.Any, 0) as EndPoint;

                        var buffer = new byte[_listenSocket.Available];
                        _listenSocket.ReceiveFrom(buffer, ref remoteEndPoint);

                        var channel = new SocketChannel();
                        channel.Assign(_listenSocket, buffer, remoteEndPoint);

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
                    }
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
                    {
                        throw;
                    }

                    retry++;
                    continue;
                }
                catch (Exception ex)
                {
                    if (ex is ObjectDisposedException || ex is NullReferenceException)
                    {
                        break;
                    }

                    OnDisconnected(ex);

                    // try again
                    continue;
                }

                Thread.Sleep(100);
            }
        }
    }
}