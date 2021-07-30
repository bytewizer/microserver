using System;
using System.Diagnostics;

using Bytewizer.TinyCLR.Http;
using Bytewizer.TinyCLR.Http.Authenticator;

using GHIElectronics.TinyCLR.Devices.Network;

namespace Bytewizer.Playground.Authentication
{
    class Program
    {
        private static HttpServer _server;

        static void Main()
        {
            StorageProvider.Initialize();
            NetworkProvider.InitializeEthernet();
            NetworkProvider.Controller.NetworkAddressChanged += NetworkAddressChanged;

            //var accountProvider = new AccountProvider();
            var accountProvider = new DefaultAccountProvider();
            accountProvider.CreateUser("admin", "password");

            _server = new HttpServer(options =>
            {
                options.Pipeline(app =>
                {
                    app.UseRouting();
                    app.UseAuthentication(new AuthenticationOptions
                    {
                        AuthenticationProvider = new DigestAuthenticationProvider(),
                        AccountProvider = accountProvider
                    });
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.Map("/", context =>
                        {
                            var username = context.GetCurrentUser().Username;
                            string response = "<doctype !html><html><head><meta http-equiv='refresh' content='1'><title>Hello, world!</title>" +
                                              "<style>body { background-color: #43bc69 } h1 { font-size:2cm; text-align: center; color: white;}</style></head>" +
                                              "<body><h1>" + DateTime.Now.Ticks.ToString() + "</h1><h1>" + username + "</h1></body></html>";

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