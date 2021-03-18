using Bytewizer.TinyCLR.Http;
using Bytewizer.TinyCLR.Hardware;

namespace Bytewizer.Playground.Mvc
{
    class Program
    {
        static void Main()
        {
            InitializeHardware();

            var server = new HttpServer(options =>
            {
                options.Pipeline(app =>
                {
                    app.UseRouting();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapDefaultControllerRoute(); // Maps root to /home/index
                        endpoints.MapControllers(); // Maps all /controller/actions
                        endpoints.MapControllerRoute(
                            name: "persons",
                            pattern: "/persons",
                            defaults: new Route { controller = "json", action = "getpersons" }
                        );
                        endpoints.Map("/money", context => 
                        {                      
                            context.Response.Write("Show me the money!"); 
                        });
                    });
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

//app.UseEndpoints(endpoints =>
//{
//    endpoints.Map("/money", context =>
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