using Bytewizer.TinyCLR.Hosting;
using Bytewizer.TinyCLR.Logging;

namespace Bytewizer.Playground.Hosting
{
    class Program
    {
        static void Main()
        {
            CreateHostBuilder().Build().Run();
        }

        public static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup(typeof(Startup));
            })
            .ConfigureLogging((context, logging) => {
                logging.SetMinimumLevel(LogLevel.Debug);
            });
    }
}