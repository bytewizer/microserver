using System;
using System.Threading;
using System.Diagnostics;
using System.Net.Sockets;

namespace Bytewizer.Sockets
{
    internal class SocketListener
    {
        private Socket _listener;
        private readonly IMiddleware _pipeline;
        private readonly SocketListenerOptions _options;

        private readonly ManualResetEvent ManualResetEvent = new ManualResetEvent(false);

        public SocketListener(SocketListenerOptions options, IMiddleware pipeline)
        {
            _options = options;
            _pipeline = pipeline;
        }

        public bool IsActive { get; private set; } = false;

        public bool Start()
        {
            if (IsActive)
                return true;

            try
            {
                _listener = new Socket(AddressFamily.InterNetwork, _options.SocketType, _options.ProtocolType);
                _listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, _options.ReuseAddress);
                _listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, _options.KeepAlive);
                if (_options.ProtocolType == ProtocolType.Udp)
                    _listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, _options.Broadcast);
                
                _listener.SendTimeout = _options.SendTimeout;
                _listener.ReceiveTimeout = _options.ReceiveTimeout;

                _listener.Bind(_options.EndPoint);
                _listener.Listen(_options.MaxCountOfPendingConnections);

                Debug.WriteLine($"Started listener on {_options.EndPoint}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unhandled exception message: { ex.Message } StackTrace: {ex.StackTrace}");
                return false;
            }

            IsActive = true;

            ManualResetEvent.Reset();

            return true;
        }

        public void AcceptConnections()
        {
            ThreadPool.SetMinThreads(_options.MaxThreads);
            ThreadPool.SetMaxThreads(_options.MaxThreads);

            while (IsActive)
            {
                int retry = 0;

                try
                {
                    var socket = _listener.Accept();

                    if (_options.MinThreads == -1 && _options.MaxThreads == -1)
                    {
                        ProcessRequest(socket);
                    }
                    else
                    {
                        ThreadPool.QueueUserWorkItem(
                            new WaitCallback(delegate (object state)
                            {
                                ProcessRequest(socket);
                            }));
                    }
                }
                catch (SocketException ex)
                {
                    var errorCode = ex.ErrorCode;

                    //The listen socket was closed
                    if (errorCode == 125 || errorCode == 89 || errorCode == 995 || errorCode == 10004 || errorCode == 10038)
                    {
                        continue;
                    }

                    if (retry > 5) throw;

                    Debug.WriteLine($"Socket exception message: { ex.Message }");

                    retry++;
                    continue;
                }
                catch (Exception ex)
                {
                    if (ex is ObjectDisposedException || ex is NullReferenceException)
                        break;

                    Debug.WriteLine($"Unhandled exception message: { ex.Message } StackTrace: {ex.StackTrace}");
                    
                    ManualResetEvent.Set();
                    continue;
                }

                ManualResetEvent.WaitOne();
            }

            if (_listener != null)
            {
                _listener.Close();
            }
        }

        internal void ProcessRequest(Socket socket)
        {
            var context = new Context();

            using (socket)
            {
                try
                {
                    if (_options.IsTls)
                    {
                        context.Assign(socket, _options.Certificate, _options.SslProtocols);
                    }
                    else
                    {
                        context.Assign(socket);
                    }

                    _pipeline.Execute(context);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Failed to accept incoming connection. Exception: { ex.Message } StackTrace: {ex.StackTrace}");
                    return;
                }
                finally
                {
                    ManualResetEvent.Set();
                }
            }
        }

        public bool Stop()
        {
            if (!IsActive)
                return true;

            IsActive = false;

            if (_listener != null)
            {
                _listener = null;
            }

            return true;
        }
    }
}