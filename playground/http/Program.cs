using Bytewizer.TinyCLR.Hardware;

using Bytewizer.TinyCLR.Http;
using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Logging.Debug;

namespace Bytewizer.Playground.Http
{
    class Program
    {
        static void Main()
        {
            var hardwareOptions = new HardwareOptions() { BoardModel = BoardModel.Sc20260D };
            var MainBoard = new Mainboard(hardwareOptions).Connect();
            MainBoard.Network.Enabled();

            var loggerFactory = new LoggerFactory();
            loggerFactory.AddDebug(LogLevel.Trace);

            var server = new HttpServer(loggerFactory, options =>
            {
                options.Pipeline(app =>
                {
                    app.UseDeveloperExceptionPage();
                    app.UseStaticFiles();
                    app.UseResponseMiddleware();
                });
            });
            server.Start();
        }
    }
}