using Bytewizer.TinyCLR.Http;
using Bytewizer.TinyCLR.Hardware;

namespace Bytewizer.Playground.Mvc
{
    class Program
    {
        public static IMainboard MainBoard;

        static void Main()
        {
            try
            {
                MainBoard = Mainboard.Connect(BoardModel.Sc20260D);

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