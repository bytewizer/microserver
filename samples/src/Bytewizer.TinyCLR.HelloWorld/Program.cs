using System;

using Bytewizer.TinyCLR.Http;

namespace Bytewizer.TinyCLR.HelloWorld
{
    class Program
    {
        static void Main()
        {
            // Initialize SC2026D development board ethernet
            //Hardware.InitializeEthernet();

            // Initialize Wifi click installed in slot 1 on SC2026D development board
            //Hardware.InitializeWiFiClick("ssid", "password");

            // Initialize WiFi on FEZ Duino and FEZ Feather
            Hardware.InitializeWiFi("ssid", "password");

            var server = new HttpServer(options =>
            {
                options.Pipeline(app =>
                {
                    app.UseRouting();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.Map("/", context =>
                        {
                            string response = "<doctype !html><html><head><title>Hello, world!</title>" +
                            "<meta http-equiv='refresh' content='5'></head><body><h1>" 
                            + DateTime.Now.Ticks.ToString() + "</h1></body></html>";

                            context.Response.Write(response);
                        });
                    });
                });
            });
            server.Start();
        }
    }
}