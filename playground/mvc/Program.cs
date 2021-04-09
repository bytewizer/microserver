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