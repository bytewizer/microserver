using System;

using Bytewizer.TinyCLR.Hosting;
using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.DependencyInjection;
using Bytewizer.TinyCLR.Hardware;
using Bytewizer.Extensions.Configuration;
using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.Http;

namespace Bytewizer.Playground.Hosting
{
    class Program
    {
        public static IHardware MainBoard;

        static void Main()
        {
            //var builder = new ConfigurationBuilder();
            //IConfigurationRoot configuration = builder.Build();
            //var settings = configuration.GetSection("AppSettings");

            MainBoard = Mainboard.Connect(BoardModel.Sc20260D);


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
                 services.AddLogging();
                 
             })
            .ConfigureLogging((context, logging) =>
            {
                logging.SetMinimumLevel(LogLevel.Warning);
            });
    }
}