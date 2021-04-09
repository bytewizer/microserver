using System;
using System.Diagnostics;
//using System.Security.Cryptography;
using System.Text;
using Bytewizer.TinyCLR.Http;

namespace Bytewizer.Playground.WebSocket
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
                    app.UseWebSockets();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapHubs();
                        endpoints.MapHub("/", typeof(ChatHub));
                    });
                });
            });
            server.Start();


        }
    }
}