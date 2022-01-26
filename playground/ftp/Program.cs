using System.Text;

using Bytewizer.TinyCLR.Ftp;
using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.Identity;
using Bytewizer.TinyCLR.Hosting;
using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Logging.Debug;

using GHIElectronics.TinyCLR.Devices.Network;
using Bytewizer.Playground.Ftp.Properties;
using System.Security.Cryptography.X509Certificates;


namespace Bytewizer.Playground.Ftp
{
    internal class Program
    {
        private static IServerService _ftpServer;
        private static readonly ILoggerFactory _loggerFactory = new LoggerFactory();

        static void Main()
        {
            StorageProvider.Initialize();
            NetworkProvider.InitializeEthernet();
            NetworkProvider.Controller.NetworkAddressChanged += NetworkAddressChanged;

            _loggerFactory.AddDebug(LogLevel.Trace);

            var X509cert = new X509Certificate(Resources.GetBytes(Resources.BinaryResources.ServerCert))
            {
                PrivateKey = Resources.GetBytes(Resources.BinaryResources.ServerKey)
            };


            //var user = new IdentityUser("bsmith");
            //var identityProvider = new IdentityProvider();

            //identityProvider.Create(user, "password");

            _ftpServer = new FtpServer(options =>
            {
                //options.UseImplicit(X509cert);
                //options.UseExplicit(X509cert);
                //options.Pipeline(app =>
                //{
                //    app.UseAuthentication(new AuthenticationOptions
                //    {
                //        IdentityProvider = identityProvider,
                //        AllowAnonymous = true
                //    }
                //);
                //});
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
                _ftpServer.Start();
            }
        }
    }
}