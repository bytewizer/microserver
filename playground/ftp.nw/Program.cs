using System.Diagnostics;

using Bytewizer.TinyCLR.Ftp;
using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Logging.Debug;

using GHIElectronics.TinyCLR.Devices.Network;

namespace Bytewizer.Playground.Ftp
{
    class Program
    {
        private static FtpServer _server;
        private static readonly ILoggerFactory loggerFactory = new LoggerFactory();

        static void Main()
        {
            StorageProvider.Initialize();
            NetworkProvider.InitializeEthernet();
            NetworkProvider.Controller.NetworkAddressChanged += NetworkAddressChanged;

            loggerFactory.AddDebug(LogLevel.Trace);

            _server = new FtpServer();
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
            }
            else
            {
                _server.Stop();
            }
        }
    }
}