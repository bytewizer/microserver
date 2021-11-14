using Bytewizer.TinyCLR.Http;
using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Logging.Debug;

using GHIElectronics.TinyCLR.Devices.Network;

namespace Bytewizer.TinyCLR.DuinoAPI
{
    class Program
    {   
        private static ILogger _logger;
        private static HttpServer _httpServer;

        private static readonly ILoggerFactory _loggerFactory = new LoggerFactory();

        static void Main()
        {
            // Set your ssid and password
            NetworkProvider.Initialize("crytek", "!therices!");
            NetworkProvider.Controller.NetworkAddressChanged += NetworkAddressChanged;

            _loggerFactory.AddDebug();
            _logger = _loggerFactory.CreateLogger(nameof(Main));

            _httpServer = new HttpServer(_loggerFactory, options =>
            {            
                options.Pipeline(app =>
                {
                    app.UseRouting();
                    app.UseEndpoints(endpoints =>
                    {
                        // Auto maps controllers to /json/getdeviceinfo /json/getnetworkinfo 
                        // endpoints.MapControllers(); 

                        // or you can create each map individually
                        endpoints.MapControllerRoute(
                            name: "deviceInfo",
                            pattern: "/device",
                            defaults: new Route { controller = "json", action = "getdeviceinfo" }
                        );
                        endpoints.MapControllerRoute(
                            name: "networkInfo",
                            pattern: "/network",
                            defaults: new Route { controller = "json", action = "getnetworkinfo" }
                        );
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
                
                // Start http service
                _httpServer.Start();
                var scheme = _httpServer.ListenerOptions.IsTls ? "https" : "http";
                _logger.LogInformation($"Launch On: {scheme}://{ipProperties.Address}:{_httpServer.ActivePort}/device");
            }
            else
            {
                // Stop http services
                _httpServer.Start();
            }
        }
    }
}
