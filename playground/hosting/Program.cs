using System;

using Bytewizer.TinyCLR.Hosting;
using Bytewizer.TinyCLR.Logging;
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
            .ConfigureServices((context, services) =>
             {
                 services.AddHostedService(typeof(MainService));
                 services.AddSingleton(typeof(IFooService), typeof(FooService));
                 services.AddSingleton(typeof(IBarService), typeof(BarService));
             })
            .ConfigureLogging((context, logging) => {
                logging.SetMinimumLevel(LogLevel.Warning);
            });
    }
}