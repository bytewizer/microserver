using System.Diagnostics;

using Bytewizer.TinyCLR.Http;
using Bytewizer.Playground.Embedded.Properties;

using GHIElectronics.TinyCLR.Devices.Network;

namespace Bytewizer.Playground.Embedded
{
    class Program
    {
        private static HttpServer _server;

        static void Main()
        {
            //StorageProvider.Initialize();

            // Write wireless settings to flash once then remove code
            //SettingsProvider.Write(new FlashObject() { Ssid = "ssid", Password = "password" });

            NetworkProvider.InitializeWiFiClick();
            NetworkProvider.Controller.NetworkAddressChanged += NetworkAddressChanged;

            var resourceManager = Resources.ResourceManager;

            _server = new HttpServer(options =>
            {
                options.Pipeline(app =>
                {
                    app.UseRouting();
                    app.UseResources(resourceManager);
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

                        endpoints.Map("/640.jpg", context =>
                        {
                            context.Response.SendResource(
                                    (short)Resources.BinaryResources.Image640,
                                    "image/jpeg",
                                    "640.jpg"
                                );
                        });

                        endpoints.Map("/1280.jpg", context =>
                        {
                            context.Response.SendResource(
                                    (short)Resources.BinaryResources.Image1280,
                                    "image/jpeg",
                                    "1280.jpg"
                                );
                        });

                        endpoints.Map("/1920.jpg", context =>
                        {
                            context.Response.SendResource(
                                    (short)Resources.BinaryResources.Image1920,
                                    "image/jpeg",
                                    "1920.jpg"
                                );
                        });

                        endpoints.Map("/", context =>
                        {
                            string response = Content();

                            context.Response.Write(response);
                        });
                    });
                });
            });
        }

        static string Content()
        {
            return @"
            <!DOCTYPE html>
            <html lang=""en"">
              <head>
                <meta charset=""utf-8"">
                <title>Bytewizer Resorces</title>
                <style>
                    img {
                        width: 640px;
                        height: 480px;
                    }
                    .item {
                        text-align: center;
                    }
                    </style>
              </head>
              <body>
                <p class=""item"">
                    <img src=""/640.jpg"" />
                </p>
                <p class=""item"">
                    <img src=""/1280.jpg"" />
                </p>
                <p class=""item"">
                    <img src=""/1920.jpg"" />
                </p>
              </body>
            </html>
            ";
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