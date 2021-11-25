using System.Text;

using Bytewizer.TinyCLR.Ftp;
using Bytewizer.TinyCLR.Identity;
using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Logging.Debug;

namespace Bytewizer.Playground.Ftp
{
    internal class Program
    {
        private static readonly ILoggerFactory _loggerFactory = new LoggerFactory();

        static void Main()
        {
            StorageProvider.Initialize();
            NetworkProvider.InitializeEthernet();

            _loggerFactory.AddDebug(LogLevel.Trace);

            var user = new IdentityUser("trice");
            var password = Encoding.UTF8.GetBytes("naidar");
            var identityProvider = new IdentityProvider();
            identityProvider.Create(user, password);

            var server = new FtpServer(_loggerFactory, options =>
            {
                options.Pipeline(app =>
                {
                    app.UseAuthentication(new AuthenticationOptions
                    {
                        IdentityProvider = identityProvider         
                    }
                );
                });
            });
            server.Start();
        }
    }
}