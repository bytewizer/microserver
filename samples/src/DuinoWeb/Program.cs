using System;

using Bytewizer.TinyCLR.Sntp;
using Bytewizer.TinyCLR.Http;
using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Logging.Debug;

using GHIElectronics.TinyCLR.Devices.Network;

namespace Bytewizer.TinyCLR.DuinoSntp
{
    class Program
    {   
        private static ILogger _logger;
        private static SntpServer _sntpServer;
        private static HttpServer _httpServer;

        private static readonly ILoggerFactory _loggerFactory = new LoggerFactory();

        static void Main()
        {
            ClockProvider.Initialize();
            
            NetworkProvider.Initialize("ssid", "password");
            //NetworkProvider.Controller.NetworkAddressChanged += NetworkAddressChanged;

            _loggerFactory.AddDebug();
            _logger = _loggerFactory.CreateLogger(nameof(Program));

            //var timesource = DateTime.UtcNow;

            _sntpServer = new SntpServer(_loggerFactory, options =>
            {
                // Set server to secondary status pulling time from an upstream server
                options.Server = "time.google.com";

                // Set realtime clock provider to get timestamp data from
                options.RealtimeClock = ClockProvider.Controller;

            });

            _httpServer = new HttpServer(_loggerFactory, options =>
            {
                options.Pipeline(app =>
                {
                    app.UseRouting();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.Map("/", context =>
                        {
                            string response = "<doctype !html><html><head><title>Network Time</title>" +
                                              "<style>body { background-color: #68829E } h1 { font-size:2cm; text-align: center; color: #505160;}</style></head>" +
                                              "<body><h1>" + DateTime.Now.ToString() + "(UTC)</h1></body></html>";

                            context.Response.Write(response);
                        });
                    });
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
                _logger.LogInformation(NetworkProvider.Info(sender));
                
                // Start sntp service
                _sntpServer.Start();

                // Start http service
                _httpServer.Start();
                var scheme = _httpServer.ListenerOptions.IsTls ? "https" : "http";
                _logger.LogInformation($"Launch On: {scheme}://{ipProperties.Address}:{_httpServer.ActivePort}");
            }
            else
            {
                // Stop all services
                _sntpServer.Stop();
                _httpServer.Start();
            }
        }
    }
}
