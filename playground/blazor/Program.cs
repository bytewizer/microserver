using Bytewizer.TinyCLR.Http;

namespace Bytewizer.Playground.Blazor
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
                    app.UseBlazorFrameworkFiles("/blazor");
                });
            });
            server.Start();
        }
    }
}