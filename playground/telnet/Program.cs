using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Terminal;
using Bytewizer.TinyCLR.Logging.Debug;

using GHIElectronics.TinyCLR.Devices.Network;
using System.Threading;

namespace Bytewizer.Playground.Terminal
{
    internal class Program
    {
        private static TelnetServer _telnetServer;

        private static ConsoleServer _consoleServer;
        private static readonly ILoggerFactory _loggerFactory = new LoggerFactory();

        static void Main()
        {
            StorageProvider.Initialize();
            NetworkProvider.InitializeEthernet();
            
            NetworkProvider.Controller.NetworkAddressChanged += NetworkAddressChanged;

            _loggerFactory.AddDebug(LogLevel.Information);

            _telnetServer = new TelnetServer(_loggerFactory, options =>
            {
                options.Pipeline(app =>
                {
                    //app.UseIpFiltering("192.168.1.0/24");
                    //app.UseAuthentication("bsmith", "password");
                    app.UseAutoMapping();
                });
            });

            _consoleServer = new ConsoleServer(_loggerFactory, options =>
            {
                //options.Pipeline(app =>
                //{

                //});
            });
            _consoleServer.Start();

        }

        private static void NetworkAddressChanged(
           NetworkController sender,
           NetworkAddressChangedEventArgs e)
        {
            var ipProperties = sender.GetIPProperties();
            var address = ipProperties.Address.GetAddressBytes();

            if (address != null && address[0] != 0 && address.Length > 0)
            {
                if (_telnetServer != null)
                {
                    _telnetServer.Start();
                }
            }
        }
    }
}