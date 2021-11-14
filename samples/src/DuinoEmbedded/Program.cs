using System;

using Bytewizer.TinyCLR.Sntp;
using Bytewizer.TinyCLR.Http;
using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Logging.Debug;

using Bytewizer.TinyCLR.DuinoEmbedded.Properties;

using GHIElectronics.TinyCLR.Devices.Network;


namespace Bytewizer.TinyCLR.DuinoEmbedded
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
            StorageProvider.Initialize();
            
            // Set your ssid and password
            NetworkProvider.Initialize("ssid", "password");
            NetworkProvider.Controller.NetworkAddressChanged += NetworkAddressChanged;

            _loggerFactory.AddDebug();
            _logger = _loggerFactory.CreateLogger(nameof(Main));

            _sntpServer = new SntpServer(_loggerFactory, options =>
            {
                // Set server to secondary status pulling time from an upstream server
                options.Server = "time.google.com";

                // Set realtime clock provider to get timestamp data from
                options.RealtimeClock = ClockProvider.Controller;

            });

            var resources = Resources.ResourceManager;

            _httpServer = new HttpServer(_loggerFactory, options =>
            {
                options.Pipeline(app =>
                {
                    app.UseRouting();
                    app.UseResource(resources);
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.Map("/favicon.ico", context =>
                        {
                            context.Response.SendResource(
                                    (short)Resources.BinaryResources.Favicon,
                                    "image/x-icon",
                                    "favicon.ico"
                                );
                        });

                        endpoints.Map("/image.jpg", context =>
                        {
                            context.Response.SendResource(
                                    (short)Resources.BinaryResources.Image,
                                    "image/jpeg",
                                    "image.jpg"
                                );
                        });

                        endpoints.Map("/", context =>
                        {
                            string response = @"
                                    <!DOCTYPE html>
                                    <html lang=""en"">
                                        <head>
                                        <meta charset=""utf-8"">
                                        <title>Bytewizer Resorces</title>
                                        <style>
                                            .item {
                                                text-align: center;
                                            }
                                            </style>
                                        </head>
                                        <body>
                                        <p class=""item"">
                                            <img src=""/image.jpg"" />
                                        </p>
                                        </body>
                                    </html>
                                    ";

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
