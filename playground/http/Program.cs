using System;

using Bytewizer.TinyCLR.Http;

namespace Bytewizer.Playground.Http
{
    class Program
    {
        static void Main()
        {
            StorageProvider.Initialize();
            NetworkProvider.InitializeEthernet();

            var server = new HttpServer(options =>
            {
                options.Pipeline(app =>
                {
                    app.UseRouting();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.Map("/", context =>
                        {
                            string response = "<doctype !html><html><head><meta http-equiv='refresh' content='1'><title>Hello, world!</title>" +
                                              "<style>body { background-color: #43bc69 } h1 { font-size:2cm; text-align: center; color: white;}</style></head>" +
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