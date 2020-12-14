using Bytewizer.TinyCLR.Http;
using Bytewizer.TinyCLR.Hosting;
using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Hardware;
using Bytewizer.TinyCLR.DependencyInjection;

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
                .ConfigureServices((context, services) =>
                {
                    //services.AddSingleton(typeof(HomeController));
                    
                    // TODO: It looks like GHI issue #525 may be blocking other services to continue
                    // https://github.com/ghi-electronics/TinyCLR-Libraries/issues/525
                    //services.AddHostedService(typeof(TimedHostedService));
                })
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder.UseFileServer();
                    webBuilder.UseMvc();
                })
                .ConfigureLogging((context, logging) =>
                {
                    logging.SetMinimumLevel(LogLevel.Warning);
                });
    }
}