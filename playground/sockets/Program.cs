using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.Hardware;

using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Logging.Debug;

namespace Bytewizer.Playground.Sockets
{
    class Program
    {
        private static readonly ILoggerFactory loggerFactory = new LoggerFactory();

        static void Main()
        {
            InitializeHardware();
            loggerFactory.AddDebug(LogLevel.Trace);

            IServer server = new SocketServer(loggerFactory, options =>
            {
                options.Pipeline(app =>
                {
                    //app.UseMemoryInfo();
                    app.UseHttpResponse();
                });
            });
            server.Start();
        }
        public static void InitializeHardware()
        {
            var hardwareOptions = new HardwareOptions() { BoardModel = BoardModel.Sc20260D };
            var mainBoard = new Mainboard(hardwareOptions).Connect();
            mainBoard.Network.Enabled();
        }
    }
}