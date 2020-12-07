using System;

using Bytewizer.TinyCLR.Hosting;
using Bytewizer.TinyCLR.Logging;
using Bytewizer.Extensions.Hardware;
using Bytewizer.TinyCLR.DependencyInjection;
using Bytewizer.TinyCLR.Hardware;

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
                 //services.AddHardware();
                 services.AddLogging();
                 
             })
            .ConfigureLogging((context, logging) =>
            {
                logging.SetMinimumLevel(LogLevel.Warning);
            });
    }
}