using Bytewizer.TinyCLR.Http;

namespace Bytewizer.TinyCLR.Sample
{
    class Program
    {
        static void Main()
        {
            Networking.SetupEthernet();

            var server = new HttpServer(options =>
            { 
                options.UseMiddleware(new HttpMiddleware());
                options.UseDeveloperExceptionPage();
                options.UseCustomMiddleware();
            });
            server.Start();
        }
    }
}