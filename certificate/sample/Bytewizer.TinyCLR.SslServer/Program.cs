using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Diagnostics;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

using GHIElectronics.TinyCLR.Pins;
using GHIElectronics.TinyCLR.Devices.Network;

using Bytewizer.TinyCLR.SslServer.Properties;

namespace Bytewizer.TinyCLR.SslServer
{
    class Program
    {
        static void Main()
        {
            // Initialize SC2026D development board ethernet
            InitializeEthernet();

            byte[] servercert = Resources.GetBytes(Resources.BinaryResources.ServerCert);

            var certificate = new X509Certificate(servercert)
            {
                PrivateKey = Resources.GetBytes(Resources.BinaryResources.ServerKey),
            };

            var server = new HttpServer(certificate);

            server.Start();
        }

        private static void InitializeEthernet()
        {
            var networkController = NetworkController.FromName(SC20260.NetworkController.EthernetEmac);

            var networkInterfaceSetting = new EthernetNetworkInterfaceSettings
            {
                MacAddress = new byte[] { 0x00, 0x8D, 0xA4, 0x49, 0xCD, 0xBD },
                Address = new IPAddress(new byte[] { 192, 168, 1, 200 }),
                SubnetMask = new IPAddress(new byte[] { 255, 255, 255, 0 }),
                GatewayAddress = new IPAddress(new byte[] { 192, 168, 1, 1 }),
                DnsAddresses = new IPAddress[] { new IPAddress(new byte[] { 192, 168, 1, 1 }) },
                DhcpEnable = false,
                DynamicDnsEnable = false
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
                Debug.WriteLine($"Lauch web brower on: https://{ipProperties.Address}");
            }
        }
    }

    public class HttpServer
    {
        private Thread _thread;
        private Socket _listener;

        private X509Certificate _certificate;

        private bool _active = false;
        private readonly ManualResetEvent _acceptEvent = new ManualResetEvent(false);
        private readonly ManualResetEvent _startedEvent = new ManualResetEvent(false);

        public HttpServer(X509Certificate certificate)
        {
            _certificate = certificate;
        }

        public void Start()
        {
            // Don't return until thread that calls Accept is ready to listen
            _startedEvent.Reset();

            // create the socket listener
            _listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            _listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

            // bind the listening socket to the port
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 443);
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
            NetworkStream networkStream;

            //networkStream = new NetworkStream(socket);

            try
            {
                SslStream sslStream = new SslStream(socket);
                sslStream.AuthenticateAsServer(_certificate, System.Security.Authentication.SslProtocols.Tls12);

                networkStream = sslStream;
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException($"Handshake was not completed within the given interval.");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to authenticate {socket.RemoteEndPoint}.", ex);
            }

            if (networkStream == null)
            {
                return;
            }

            using (var reader = new StreamReader(networkStream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Debug.WriteLine(line);
                }

                string response = "HTTP/1.1 200 OK\r\nContent-Type: text/html; charset=UTF-8\r\nConnection: close\r\n\r\n" +
                                         "<doctype !html><html><head><title>Hello, world!</title>" +
                                         "<style>body { background-color: #111 } h1 { font-size:2cm; text-align: center; color: white;}</style></head>" +
                                         "<body><h1>" + DateTime.Now.Ticks.ToString() + "</h1></body></html>";

                var bytes = Encoding.UTF8.GetBytes(response);
                networkStream.Write(bytes, 0, bytes.Length);
            }
        }
    }
}