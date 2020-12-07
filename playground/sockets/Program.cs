using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.Hardware;

namespace Bytewizer.Playground.Sockets
{
    class Program
    {
        private static IHostHardware MainBoard;

        static void Main()
        {
            try
            {
                MainBoard = Mainboard.Connect(BoardModel.Sc20260D);

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