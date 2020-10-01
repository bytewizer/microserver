using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Diagnostics;

namespace Bytewizer.TinyCLR.BindError
{
    class Program
    {
        private static Thread _thread;
        private static Socket _listener;

        private static bool _active = false;
        private static readonly ManualResetEvent _accept = new ManualResetEvent(false);
        private static readonly ManualResetEvent _started = new ManualResetEvent(false);

        static void Main()
        {
            // Initialize wifi
            NetworkProvider.Initialize();

            Start();
            //Thread.Sleep(5000);
            //Stop();
            //NetworkProvider.Controller.Disable();
            //Thread.Sleep(5000);
            //NetworkProvider.Controller.Enable();
            //Start();
        }

        public static void Start() 
        {
            // Don't return until thread that calls Accept is ready to listen
            _started.Reset();

            // create the socket listener
            _listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            _listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

            // bind the listening socket to the port
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 80);
            _listener.Bind(endPoint);

            // start listening
            _listener.Listen(10);

            _thread = new Thread(() =>
            {
                _active = true;
                AcceptConnections();
            });
            _thread.Priority = ThreadPriority.AboveNormal;
            _thread.Start();

            // Waits for thread that calls Accept() to start
            _started.WaitOne();

            Debug.WriteLine($"Started socket listener on {_listener.LocalEndPoint}");
        }

        public static void Stop()
        {
            _active = false;

            // Signal the main thread to continue
            _accept.Set();
            
            // Wait for thread to exit 
            _thread.Join(1000);
            _thread = null;

            _listener.Close();
            _listener = null;

            Debug.WriteLine("Stopped socket listener");
        }
        
        private static void AcceptConnections()
        {
            // Set the started event to signaled state
            _started.Set();

            while (_active)
            {             
                // Set the accept event to nonsignaled state
                _accept.Reset();

                Debug.WriteLine("Waiting for a connection...");
                using (var socket = _listener.Accept())
                {
                    // Set the accept event to signaled state
                    _accept.Set();
                    
                    // Send response to client
                    Response(socket);

                    // Close connection
                    socket.Close();
                }
                
                // Wait until a connection is made before continuing
                _accept.WaitOne();           
            }

            Debug.WriteLine("Exited AcceptConnection()");
        }
    
        private static void Response(Socket socket)
        {
            var network = new NetworkStream(socket);
            var reader = new StreamReader(network);

            while (reader.Peek() != -1)
            {
                var line = reader.ReadLine();
                Debug.WriteLine(line);
            }

            string response = "HTTP/1.1 200 OK\r\nContent-Type: text/html; charset=UTF-8\r\nConnection: close\r\n\r\n" +
                                 "<doctype !html><html><head><title>Hello, world!</title>" +
                                 "<style>body { background-color: #111 } h1 { font-size:2cm; text-align: center; color: white;}</style></head>" +
                                 "<body><h1>" + DateTime.Now.Ticks.ToString() + "</h1></body></html>";
            
            var bytes = Encoding.UTF8.GetBytes(response);
            network.Write(bytes, 0, bytes.Length);
        }
    }
}