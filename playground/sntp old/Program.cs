using System;
using System.Threading;

using Bytewizer.TinyCLR.Sntp;
using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Logging.Debug;

namespace Bytewizer.Playground.Sntp
{
    class Program
    {
        private static readonly ILoggerFactory loggerFactory = new LoggerFactory();

        static void Main()
        {
            ClockProvider.Initialize();
            NetworkProvider.InitializeWiFiClick("crytek","!therices!");
            //NetworkProvider.InitializeEthernet();

            Thread.Sleep(10000);

            loggerFactory.AddDebug(LogLevel.Debug);

            var server = new SntpServer(loggerFactory, options =>
            {                
                // Set server to secondary status pulling time from an upstream server
                options.Server = "pool.ntp.org";

                // Set time source to realtime clock time date and time
                options.TimeSource = ClockProvider.Controller.Now;

                options.SyncInterval = TimeSpan.FromSeconds(60);

            });
            server.Start();
        }
    }
}