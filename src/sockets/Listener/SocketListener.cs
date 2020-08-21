using System;
using System.Threading;
using System.Diagnostics;
using System.Net.Sockets;

using Bytewizer.TinyCLR.Threading;

namespace Bytewizer.TinyCLR.Sockets
{
    public class SocketListener
    {
        private Thread thread;
        private Socket listener;
        
        private readonly ManualResetEvent acceptEvent = new ManualResetEvent(false);  
        private readonly SocketListenerOptions options;

        public SocketListener(SocketListenerOptions options)
        {
            this.options = options;

            listener = new Socket(AddressFamily.InterNetwork, options.SocketType, options.ProtocolType);
            listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, options.ReuseAddress);
            listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, options.KeepAlive);

            if (options.ProtocolType == ProtocolType.Udp)
                listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, options.Broadcast);

            listener.SendTimeout = options.SendTimeout;
            listener.ReceiveTimeout = options.ReceiveTimeout;

            ThreadPool.SetMinThreads(options.MaxThreads);
            ThreadPool.SetMaxThreads(options.MaxThreads);
        }
     
        public bool IsActive { get; private set; } = false;

        public bool Start()
        {
            if (IsActive)
                return true;

            try
            {
                // Bind the socket to the local endpoint and listen for incoming connections
                listener.Bind(options.EndPoint);
                listener.Listen(options.MaxCountOfPendingConnections);
       
                thread = new Thread(() =>
                {
                    if (listener != null)
                    {
                        IsActive = true;
                        AcceptConnections();
                    }
                });
                thread.Priority = options.ThreadPriority;
                thread.Start();

                Debug.WriteLine($"Started SocketServer listener on {options.EndPoint}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unhandled exception message: { ex.Message } StackTrace: {ex.StackTrace}");
                IsActive = false;
                return false;
            }

            return true;
        }

        public bool Stop()
        {
            if (!IsActive)
                return true;

            IsActive = false;

            if (thread != null)
            {
                // Wait for thread to exit
                thread.Join(100);
                thread = null;
            }

            if (listener != null)
            {
                // Dispose of listener
                listener.Close();
                listener = null;
            }

            ThreadQueue.Instance.Dispose();

            acceptEvent.Set();

            Debug.WriteLine($"Stopped SocketServer listener");

            return true;
        }

        public void AcceptConnections()
        {
            while (IsActive)
            {
                int retry = 0;

                try
                {
                    // Set the event to nonsignaled state
                    acceptEvent.Reset();

                    var socket = listener.Accept();
                      
                    ThreadQueue.Instance.Enqueue(() => {
                        
                        // Signal the main thread to continue
                        acceptEvent.Set();

                        ClientConnected(socket);
                    });

                    // Wait until a connection is made before continuing
                    acceptEvent.WaitOne();
                }
                catch (SocketException ex)
                {
                    var errorCode = ex.ErrorCode;

                    //The listen socket was closed
                    if (errorCode == 125 || errorCode == 89 || errorCode == 995 || errorCode == 10004 || errorCode == 10038)
                    {
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

                    Debug.WriteLine($"Unhandled exception message: { ex.Message } StackTrace: {ex.StackTrace}");

                    acceptEvent.Set();
                  
                    continue;
                }
            }

            if (listener != null)
            {
                listener.Close();
                listener = null;
            }
        }

        //internal void ProcessRequest(Socket socket)
        //{
        //    // Signal the main thread to continue
        //    acceptEvent.Set();

        //    ClientConnected(socket);
        //}

        public event ConnectedEventHandler ClientConnected = delegate { };
        public delegate void ConnectedEventHandler(Socket socket);
    }
}