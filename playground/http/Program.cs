using System;
using Bytewizer.TinyCLR.Http;
using Bytewizer.TinyCLR.Hardware;

namespace Bytewizer.Playground.Http
{
    class Program
    {
        static void Main()
        {
            var hardwareOptions = new HardwareOptions() { BoardModel = BoardModel.Sc20260D };
            var MainBoard = new Mainboard(hardwareOptions).Connect();
            MainBoard.Network.Enabled();

            var server = new HttpServer(options =>
            {
                options.Pipeline(app =>
                {
                    app.UseRouting();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.Map("/", context => // Mapped to root url
                        {
                            string response = "<doctype !html><html><head><title>Hello, world!</title></head>" +
                                                "<body><h1>" + DateTime.Now.Ticks.ToString() + "</h1></body></html>";

                            context.Response.Write(response);
                        });
                    });
                });
                options.Listen(8080); 
            });
            server.Start();
        }
    }
}