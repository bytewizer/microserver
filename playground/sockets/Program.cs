using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.Hardware;

using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Logging.Debug;

namespace Bytewizer.Playground.Sockets
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

            IServer server = new SocketServer(loggerFactory, options =>
            {
                options.Pipeline(app =>
                {
                    app.UseMemoryInfo();
                    app.UseHttpResponse();
                });
            });
            server.Start();
        }
    }
}