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
                    app.UseRouting();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllerRoute(
                            name: "test",
                            pattern: "/getok",
                            defaults: new Route { controller = "test", action = "getok" }
                        );
                        endpoints.Map("/money", context => 
                        {
                            context.Response.Write("Show me the money!"); 
                        });
                        endpoints.MapDefaultControllerRoute();
                        endpoints.MapControllers();
                    });
                });
            });
            server.Start();
        }
    }
}

//app.UseEndpoints(endpoints =>
//{
//    endpoints.Map("/tom", context =>
//    {
//        context.Response.Write("Show me the money!");
//    });
//    endpoints.MapDefaultControllerRoute();
//    endpoints.MapControllers();
//    endpoints.MapControllerRoute(
//        name: "default",
//        pattern: "/index",
//        defaults: new Route { controller = "Home", action = "Index" });
//    endpoints.MapControllerRoute(
//        name: "short",
//        pattern: "/home/short",
//        defaults: new Route { controller = "Home", action = "Short" });
//});