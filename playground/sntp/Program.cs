using System;
using System.Threading;
using System.Diagnostics;

using Bytewizer.TinyCLR.Sntp;
using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Logging.Debug;

using GHIElectronics.TinyCLR.Native;

namespace Bytewizer.Playground.Sntp
{
    class Program
    {
        private static DateTime _timeSource;
        private static readonly ILoggerFactory loggerFactory = new LoggerFactory();

        static void Main()
        {
            NetworkProvider.InitializeEthernet();

            // Get the time close to current time
            var time = new DateTime(2021, 7, 5, 18, 18, 0);
            SystemTime.SetTime(time);

            // Wait for network to connect then set time every 10 seconds
            Timer timer = new Timer(SetTime, null, 10000, 10000);

            loggerFactory.AddDebug(LogLevel.Information);

            var server = new SntpServer(loggerFactory, options =>
            {
                // Local time source 
                //options.TimeSource = _timeSource; 
                //options.Stratum = Stratum.Secondary;

                // Relay from remote time source
                options.Server = "pool.ntp.org";

            }).Start();
        }

        static void SetTime(object obj)
        {
            using (var ntp = new NtpClient("pool.ntp.org", 123)) // This could be set by GPS time
            {
                ntp.Timeout = TimeSpan.FromSeconds(5);
                _timeSource = DateTime.UtcNow + ntp.GetCorrectionOffset();
                SystemTime.SetTime(_timeSource);
                Debug.WriteLine(NtpPacket.Print(ntp.Query()));
            }
        }
    }
}