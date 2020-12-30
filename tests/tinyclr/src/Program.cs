using Bytewizer.TinyCLR.Http;

namespace Bytewizer.TinyCLR.TestHarness
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
                    app.UseStaticFiles();
                    app.UseRouting();
                    app.UseTestHarness();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllers();
                    });
                });
            });
            server.Start();
        }
    }
}