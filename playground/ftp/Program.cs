using System;
using System.Collections;
using System.Diagnostics;
using System.Text;
using System.Threading;

using Bytewizer.TinyCLR.Ftp;
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

            _loggerFactory.AddDebug(LogLevel.Debug);

            var server = new FtpServer(_loggerFactory, options =>
            {
                options.AllowAnonymous = true;
            });
            server.Start();
        }
    }
}
