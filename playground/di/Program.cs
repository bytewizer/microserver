// https://github.com/grumpydev/TinyIoC/blob/master/src/TinyIoC/TinyIoC.cs
// https://github.com/microsoft/MinIoC/blob/master/Container.cs#L224
// https://github.com/dotnet/runtime/tree/master/src/libraries/Microsoft.Extensions.DependencyInjection.Abstractions/src
// https://ruijarimba.wordpress.com/2013/10/28/implementing-a-basic-ioc-container-using-csharp/
// https://docs.servicestack.net/simple-ioc#fast-small-minimal-dependency-ioc
// https://github.com/seesharper/LightInject/blob/master/src/LightInject/LightInject.cs
// https://melgrubb.com/2008/11/03/itty-bitty-ioc-a-c-ioc-container-in-100-lines/

using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Logging.Debug;
using Bytewizer.TinyCLR.DependencyInjection;

namespace Bytewizer.Playground.DependencyInjection
{
    class Program
    {     
        static void Main()
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging(b => b.AddDebug())
                .AddSingleton(typeof(IFooService), typeof(FooService))
                .AddSingleton(typeof(IBarService), typeof(BarService))
                .BuildServiceProvider();

            var loggerFactory = (LoggerFactory)serviceProvider.GetService(typeof(ILoggerFactory));
            loggerFactory.AddProvider(new DebugLoggerProvider());

            var logger = loggerFactory.CreateLogger(typeof(Program));
            logger.LogInformation("Starting application");

            //do the actual work here
            var bar = (BarService)serviceProvider.GetService(typeof(IBarService));
            bar.DoSomeRealWork();

            logger.LogInformation("All done!");
        }
    }

    public interface IFooService
    {
        void DoThing(int number);
    }

    public interface IBarService
    {
        void DoSomeRealWork();
    }

    public class BarService : IBarService
    {
        private readonly IFooService _fooService;
        
        public BarService(IFooService fooService)
        {
            _fooService = fooService;
        }

        public void DoSomeRealWork()
        {
            for (int i = 0; i < 10; i++)
            {
                _fooService.DoThing(i);
            }
        }
    }

    public class FooService : IFooService
    {
        private readonly ILogger _logger;
        
        public FooService(ILoggerFactory loggerFactory)
        {
            //loggerFactory.AddProvider(new DebugLoggerProvider(LogLevel.Trace));
            _logger = loggerFactory.CreateLogger(typeof(FooService));
        }

        public void DoThing(int number)
        {
            _logger.LogInformation($"Doing the thing {number}");
        }
    }
}