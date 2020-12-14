using Bytewizer.TinyCLR.Hosting;
using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Hardware;
using Bytewizer.TinyCLR.DependencyInjection;

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
            .ConfigureHardware(config =>
            {
                config.BoardModel = BoardModel.Sc20260D;
            })
            .ConfigureHostConfiguration(config =>
            {
            })
            .ConfigureServices((context, services) =>
            {

                services.AddHostedService(typeof(TimedHostedService));
                services.AddHostedService(typeof(WorkerService));
                services.AddHostedService(typeof(MainService));
                services.AddSingleton(typeof(IFooService), typeof(FooService));
                services.AddSingleton(typeof(IBarService), typeof(BarService));
                services.AddLogging();

            })
            .ConfigureLogging((context, logging) =>
            {
                logging.SetMinimumLevel(LogLevel.Warning);
            });
        
    }
}