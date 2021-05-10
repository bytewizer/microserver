using System;
using System.Net;
using System.Threading;
using System.Net.Sockets;

using Bytewizer.TinyCLR.Sockets.Channel;
using Bytewizer.TinyCLR.Sockets.Extensions;

namespace Bytewizer.TinyCLR.Sockets.Listener
{
    /// <summary>
    /// Represents an implementation of the <see cref="SocketListener"/> which listens for remote UDP clients.
    /// </summary>
    public class UdpListener : SocketListener
    {
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
                    if (_listenSocket.Poll(-1, SelectMode.SelectRead))
                    {
                        if (_listenSocket.Available == 0)
                            return;

                        var remoteEndPoint = new IPEndPoint(0, 0) as EndPoint;
                        var buffer = new byte[_listenSocket.Available];
                        _listenSocket.ReceiveFrom(buffer, SocketFlags.None, ref remoteEndPoint);

                        var channel = new SocketChannel();
                        if (_options.IsTls)
                        {

                        }
                        else
                        {
                            channel.Assign(_listenSocket, buffer, remoteEndPoint);
                        }

                        ThreadPool.QueueUserWorkItem(
                        new WaitCallback(delegate (object state)
                        {
                            // Signal the accept thread to continue
                            _acceptEvent.Set();

                            // Invoke the connected handler
                            OnConnected(channel);

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
                        throw;

                    retry++;
                    continue;
                }
                catch (Exception ex)
                {
                    if (ex is ObjectDisposedException || ex is NullReferenceException)
                        break;

                    OnDisconnected(ex);

                    // try again
                    continue;
                }
            }
        }
    }
}
