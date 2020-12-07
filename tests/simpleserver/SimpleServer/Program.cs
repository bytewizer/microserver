using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Diagnostics;

using GHIElectronics.TinyCLR.Pins;
using GHIElectronics.TinyCLR.Devices.Network;

namespace Bytewizer.TinyCLR.WebServer
{
    class Program
    {
        static void Main()
        {
            // Initialize SC2026D development board ethernet
            InitializeEthernet();

            var server = new HttpServer();

            server.Start(); // Socket is created
            server.Stop();  // Once socket processes an http requests sockets will be destroyed
            server.Start(); // Socket will fail on this restart method
        }

        private static void InitializeEthernet()
        {
            var networkController = NetworkController.FromName(SC20260.NetworkController.EthernetEmac);

            var networkInterfaceSetting = new EthernetNetworkInterfaceSettings
            {
                MacAddress = new byte[] { 0x00, 0x8D, 0xA4, 0x49, 0xCD, 0xBD }
            };

            networkController.SetInterfaceSettings(networkInterfaceSetting);
            networkController.NetworkAddressChanged += NetworkAddressChanged;
            networkController.SetAsDefaultController();

            networkController.Enable();
        }

        private static void NetworkAddressChanged(
            NetworkController sender,
            NetworkAddressChangedEventArgs e)
        {
            var ipProperties = sender.GetIPProperties();
            var address = ipProperties.Address.GetAddressBytes();

            if (address != null && address[0] != 0 && address.Length > 0)
            {
                Debug.WriteLine($"Lauch web brower on: http://{ipProperties.Address}");
            }
        }
    }

    public class HttpServer
    {
        private Thread _thread;
        private Socket _listener;

        private bool _active = false;
        private readonly ManualResetEvent _acceptEvent = new ManualResetEvent(false);
        private readonly ManualResetEvent _startedEvent = new ManualResetEvent(false);

        public void Start()
        {
            // Don't return until thread that calls Accept is ready to listen
            _startedEvent.Reset();

            // create the socket listener
            _listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            _listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

            // bind the listening socket to the port
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 80);
            _listener.Bind(endPoint);

            // start listening
            _listener.Listen(5);

            _thread = new Thread(() =>
            {
                _active = true;
                AcceptConnections();
            });
            _thread.Priority = ThreadPriority.AboveNormal;
            _thread.Start();

            // Waits for thread that calls Accept() to start
            _startedEvent.WaitOne();

            Debug.WriteLine($"Started socket listener");
        }

        public void Stop()
        {
            _active = false;

            // Signal the accept thread to continue
            _acceptEvent.Set();

            // Wait for thread to exit 
            _thread.Join();
            _thread = null;

            _listener.Close();
            _listener = null;

            Debug.WriteLine("Stopped socket listener");
        }

        private void AcceptConnections()
        {
            // Set the started event to signaled state
            _startedEvent.Set();

            while (_active)
            {
                // Set the accept event to nonsignaled state
                _acceptEvent.Reset();

                Debug.WriteLine("Waiting for a connection...");
                using (var remoteSocket = _listener.Accept())
                {
                    // Set the accept event to signaled state
                    _acceptEvent.Set();

                    // Send response to client
                    Response(remoteSocket);

                    // Close connection
                    remoteSocket.Close();
                }

                // Wait until a connection is made before continuing
                _acceptEvent.WaitOne();
            }

            Debug.WriteLine("Exited AcceptConnection()");
        }

        private void Response(Socket socket)
        {
            using (var network = new NetworkStream(socket))
            {
                using (var reader = new StreamReader(network))
                {
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
    }
}
