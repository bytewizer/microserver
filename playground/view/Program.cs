using Bytewizer.TinyCLR.Http;
using Bytewizer.TinyCLR.Hardware;

namespace Bytewizer.Playground.Mvc
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
                    app.UseDeveloperExceptionPage();
                    app.UseRouting();
                    app.UseEndpoints(endpoints => 
                    {
                        endpoints.MapControllers();
                        //endpoints.MapDefaultControllerRoute();
                    });
                });
            });
            server.Start();
        }
    }
}