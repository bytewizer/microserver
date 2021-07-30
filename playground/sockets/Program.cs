using System.Diagnostics;

using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Logging.Debug;

using GHIElectronics.TinyCLR.Devices.Network;

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

            _server = new SocketServer(loggerFactory, options =>
            {
                options.Listen(8080);
                options.Pipeline(app =>
                {
                    //app.UseMemoryInfo();
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