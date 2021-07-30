using System;
using System.Diagnostics;

using Bytewizer.TinyCLR.Http;

using GHIElectronics.TinyCLR.Devices.Network;

namespace Bytewizer.Playground.Mvc
{
    class Program
    {
        private static HttpServer _server;

        static void Main()
        {
            StorageProvider.Initialize();
            NetworkProvider.InitializeEthernet();
            NetworkProvider.Controller.NetworkAddressChanged += NetworkAddressChanged;

            _server = new HttpServer(options =>
            {
                options.Pipeline(app =>
                {
                    app.UseRouting();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapDefaultControllerRoute(); // Maps root to /home/index
                        endpoints.MapControllers(); // Maps all /controller/actions
                        endpoints.MapControllerRoute(
                            name: "persons",
                            pattern: "/persons",
                            defaults: new Route { controller = "json", action = "getpersons" }
                        );
                        endpoints.Map("/money", context => 
                        {                      
                            context.Response.Write("Show me the money!"); 
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
                Debug.WriteLine(NetworkProvider.Info(sender));

                _server.Start();

                var scheme = _server.ListenerOptions.IsTls ? "https" : "http";
                Debug.WriteLine($"Launch On: {scheme}://{ipProperties.Address}:{_server.ActivePort}");
            }
            else
            {
                _server.Stop();
            }
        }
    }
}