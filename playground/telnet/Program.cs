using Bytewizer.TinyCLR.Telnet;
using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Identity;
using Bytewizer.TinyCLR.Logging.Debug;

using GHIElectronics.TinyCLR.Devices.Network;

namespace Bytewizer.Playground.Telnet
{
    internal class Program
    {
        private static IServer _telnetServer;
        private static readonly ILoggerFactory _loggerFactory = new LoggerFactory();

        static void Main()
        {
            StorageProvider.Initialize();
            NetworkProvider.InitializeEthernet();
            NetworkProvider.Controller.NetworkAddressChanged += NetworkAddressChanged;

            _loggerFactory.AddDebug(LogLevel.Trace);

            var user = new IdentityUser("bsmith");
            var identityProvider = new IdentityProvider();

            identityProvider.Create(user, "password");

            _telnetServer = new TelnetServer(_loggerFactory, options =>
            {
                options.Pipeline(app =>
                {
                    //app.UseAuthentication(new AuthenticationOptions
                    //{
                    //    IdentityProvider = identityProvider
                    //});
                    app.UseCommands();
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
                if (_telnetServer != null)
                {
                    _telnetServer.Start();
                }
            }
        }
    }
}