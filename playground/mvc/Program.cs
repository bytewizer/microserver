using Bytewizer.TinyCLR.Http;

namespace Bytewizer.Playground.Mvc
{
    class Program
    {
        static void Main()
        {
            StorageProvider.Initialize();
            NetworkProvider.InitializeEthernet();

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