using System;

using Bytewizer.TinyCLR.Http;

namespace Bytewizer.TinyCLR.HelloWorld
{
    class Program
    {
        static void Main()
        {
            // Before first build right click on Solution and "Restore Nuget Packages" then "Clean Solution".  
            
            // Initialize SC2026D development board ethernet
            Hardware.InitializeEthernet();

            // Initialize Wifi click installed in slot 1 on SC2026D development board
            //Hardware.InitializeWiFiClick("ssid", "password");

            // Initialize WiFi on FEZ Portal
            //Hardware.InitializeWiFi("ssid", "password");

            var server = new HttpServer(options =>
            {
                options.Pipeline(app =>
                {
                    app.UseRouting();
                    app.UseDebugHeaders();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapDefaultControllerRoute(); // Maps root to /home/index
                        endpoints.MapControllers(); // Maps all controller actions
                        endpoints.MapControllerRoute(
                            name: "persons",
                            pattern: "/persons",
                            defaults: new Route { controller = "json", action = "getpersons" }
                        );

                        endpoints.Map("/ticks", context =>
                        {
                            string response = "<doctype !html><html><head><meta http-equiv='refresh' content='1'><title>Hello, world!</title>" +
                                              "<style>body { background-color: #00203FFF } h1 { font-size:2cm; text-align: center; color: #ADEFD1FF;}</style></head>" +
                                              "<body><h1>" + DateTime.Now.Ticks.ToString() + "</h1></body></html>";

                            context.Response.Write(response);
                        });
                    });
                });
            });
            server.Start();
        }
    }
}