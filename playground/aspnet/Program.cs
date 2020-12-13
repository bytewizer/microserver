using Bytewizer.TinyCLR.Http;
using Bytewizer.TinyCLR.Hosting;
using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Hardware;

namespace Bytewizer.Playground.AspNet
{
    class Program
    {
        static void Main()
        {
            CreateHostBuilder().Build().Run();
        }

        public static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
                .ConfigureHardware(config => 
                {
                    config.BoardModel = BoardModel.Sc20260D;
                })
                .ConfigureWebHost(options =>
                {
                    options.UseDeveloperExceptionPage();
                    options.UseFileServer();
                    options.UseMvc();
                })
                .ConfigureServices((context, services) =>
                 {
                    //services.AddHostedService(typeof(TimedHostedService));
                })
                .ConfigureLogging((context, logging) =>
                {
                    logging.SetMinimumLevel(LogLevel.Warning);
                });
    }
}