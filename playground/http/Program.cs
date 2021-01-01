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
            var mainBoard = new Mainboard(hardwareOptions).Connect();
            mainBoard.Network.Enabled();

            var server = new HttpServer(options =>
            {
                options.Pipeline(app =>
                {
                    app.UseMemoryInfo();
                    app.UseRouting();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.Map("/", context =>
                        {
                            string response = "<doctype !html><html><head><title>Hello, world!" +
                                "</title><meta http-equiv='refresh' content='5'></head><body>" +
                                "<h1>" + DateTime.Now.Ticks.ToString() + "</h1></body></html>";

                            context.Response.Write(response);
                        });
                    });
                });
            });
            server.Start();
        }
    }
}