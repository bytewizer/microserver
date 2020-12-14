using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.Hardware;

namespace Bytewizer.Playground.Sockets
{
    class Program
    {
        private static IHardware MainBoard;

        static void Main()
        {
            try
            {
                var hardwareOptions = new HardwareOptions() { BoardModel = BoardModel.Sc20260D };
                MainBoard = new Mainboard(hardwareOptions).Connect();
                MainBoard.Network.Enabled();

                var server = new SocketServer(options =>
                {
                    options.Register(new HttpResponse());
                });
                server.Start();
            }
            catch
            {
                MainBoard?.Dispose();
            }
        }
    }
}