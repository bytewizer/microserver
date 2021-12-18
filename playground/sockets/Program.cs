using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Logging.Debug;
using Bytewizer.Playground.Sockets.Properties;

using GHIElectronics.TinyCLR.Devices.Network;
using System.Net;

namespace Bytewizer.Playground.Sockets
{
    class Program
    {
        private static SocketServer _server;
        private static readonly ILoggerFactory loggerFactory = new LoggerFactory();

        static void Main()
        {
            
            StorageProvider.Initialize();
            NetworkProvider.InitializeEthernet();
            NetworkProvider.Controller.NetworkAddressChanged += NetworkAddressChanged;

            loggerFactory.AddDebug(LogLevel.Trace);

            byte[] servercert = Resources.GetBytes(Resources.BinaryResources.ServerCert);

            var X509cert = new X509Certificate(servercert)
            {
                PrivateKey = Resources.GetBytes(Resources.BinaryResources.ServerKey)
            };

            _server = new SocketServer(loggerFactory, options =>
            {
                options.Listen(443, listener =>
                {
                    listener.UseTls(X509cert);
                });
                options.Pipeline(app =>
                {
                    app.UseIpFiltering("192.168.1.0/24");
                    app.UseHttpResponse();
                });
            });
        }
        private static void NetworkAddressChanged(
          NetworkController sender,
          NetworkAddressChangedEventArgs e)
        {
            var ipProperties = sender.GetIPProperties();
            var address = ipProperties.Address.GetAddressBytes();

            if (address != null && address[0] != 0 && address.Length > 0)
            {
                Debug.WriteLine(NetworkProvider.Info(sender));

                _server.Start();

                var scheme = _server.ListenerOptions.IsTls ? "https" : "http";
                Debug.WriteLine($"Launch On: {scheme}://{ipProperties.Address}:{_server.ActivePort}");
            }
            else
            {
                _server.Stop();
            }
        }
    }
}