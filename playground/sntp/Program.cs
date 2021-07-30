using System;
using System.Text;

using Bytewizer.TinyCLR.Sntp;
using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Logging.Debug;

using GHIElectronics.TinyCLR.Devices.Network;

namespace Bytewizer.Playground.Sntp
{
    class Program
    {
        private static IServer _server;
        private static ILogger _logger;
        private static readonly ILoggerFactory _loggerFactory = new LoggerFactory();

        static void Main()
        {
            ClockProvider.Initialize();
            NetworkProvider.InitializeEthernet();
            NetworkProvider.Controller.NetworkAddressChanged += NetworkAddressChanged;

            _loggerFactory.AddDebug(LogLevel.Debug);
            _logger = _loggerFactory.CreateLogger("Bytewizer.Playground.Time");

            var timesource = DateTime.UtcNow;

            _server = new SntpServer(_loggerFactory, options =>
            {
                // Set server to secondary status pulling time from an upstream server
                options.Server = "time.google.com";

                // Set realtime clock provider to get timestamp data from
                options.RealtimeClock = ClockProvider.Controller;

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
                _logger.LogInformation(NetworkProvider.Info(sender));
                _server.Start();
            }
            else
            {
                _server.Stop();
            }
        }
    }
}