using Bytewizer.TinyCLR.Http;
using Bytewizer.TinyCLR.Hardware;

namespace Bytewizer.TinyCLR.WebServer
{
    class Program
    {
        private static IMainboard _board;

        static void Main()
        {
            try
            {
                _board = Mainboard.Initialize();
                
                var server = new HttpServer(options =>
                {
                    options.UseMiddleware(new HttpMiddleware());
                    options.UseDeveloperExceptionPage();
                    options.UseFileServer();
                    options.UseMvc();
                });
                server.Start();
            }
            catch
            {
                _board?.Dispose();
            }
        }
    }
}