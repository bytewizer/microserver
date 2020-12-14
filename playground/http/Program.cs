using Bytewizer.TinyCLR.Http;
using Bytewizer.TinyCLR.Hardware;

namespace Bytewizer.Playground.Mvc
{
    class Program
    {
        public static IHardware MainBoard;

        static void Main()
        {
            try
            {
                var hardwareOptions = new HardwareOptions() { BoardModel = BoardModel.Sc20260D };
                MainBoard = new Mainboard(hardwareOptions).Connect();
                MainBoard.Network.Enabled();

                var server = new HttpServer(options =>
                {
                    options.UseDeveloperExceptionPage();
                    options.UseFileServer();
                    options.UseMvc();
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