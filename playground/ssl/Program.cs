using System;
using System.Diagnostics;

using Bytewizer.TinyCLR.Http;
using Bytewizer.Playground.Ssl.Properties;

using GHIElectronics.TinyCLR.Devices.Network;
using Bytewizer.TinyCLR.Sockets;
using System.Security.Cryptography.X509Certificates;

namespace Bytewizer.Playground.Ssl
{
    class Program
    {
        private static HttpServer _server;

        static void Main()
        {
            StorageProvider.Initialize();

            // Write wireless settings to flash once then remove code
            //SettingsProvider.Write(new FlashObject() { Ssid = "ssid", Password = "password" });

            NetworkProvider.InitializeEthernet();
            NetworkProvider.Controller.NetworkAddressChanged += NetworkAddressChanged;

            var icon = Resources.GetBytes(Resources.BinaryResources.Favicon);

            var X509cert = new X509Certificate(Resources.GetBytes(Resources.BinaryResources.ServerCert))
            {
                PrivateKey = Resources.GetBytes(Resources.BinaryResources.ServerKey)
            };

            _server = new HttpServer(options =>
            {
                options.Listen(443, listener =>
                {
                    listener.UseCert(X509cert);
                });
                options.Pipeline(app =>
                {
                    app.UseRouting();
                    app.UseEndpoints(endpoints =>
                    {          
                        endpoints.Map("/favicon.ico", context =>
                        {
                            context.Response.SendFile(icon, "image/x-icon", "favicon.ico");
                        });

                        endpoints.Map("/", context =>
                        {
                            string response = "<doctype !html><html><head><meta http-equiv='refresh' content='1'><title>Hello, world!</title>" +
                                 "<style>body { background-color: #68829E } h1 { font-size:2cm; text-align: center; color: #505160;}</style></head>" +
                                 "<body><h1>" + DateTime.Now.Ticks.ToString() + "</h1></body></html>";

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