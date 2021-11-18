using System;
using System.Diagnostics;

using Bytewizer.TinyCLR.Http;
using Bytewizer.TinyCLR.Http.PageBuilder;

using Bytewizer.Playground.PageBuilder.Properties;

using GHIElectronics.TinyCLR.Devices.Network;

namespace Bytewizer.Playground.PageBuilder
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

            var resources = Resources.ResourceManager;

            _server = new HttpServer(options =>
            {
                options.Pipeline(app =>
                {
                    app.UseRouting();
                    app.UseResources(resources);
                    app.UseEndpoints(endpoints =>
                    {          
                        endpoints.Map("/favicon.ico", context =>
                        {
                            context.Response.SendResource((short)Resources.BinaryResources.Favicon, "image/x-icon", "favicon.ico");
                        });

                        endpoints.Map("/", context =>
                        {
                            var page = new HtmlPage();
                            page.Head.Title = "Hello, world!";
                            page.Head.AdditionalElements.Add("<meta http-equiv='refresh' content='5'>");
                            page.Head.Style = "body { background-color: #68829E } h1 { font-size:2cm; text-align: center; color: #505160;}";
                            page.Body.Content += page.Body.H1Text(DateTime.Now.Ticks.ToString());

                            context.Response.Write(page);
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