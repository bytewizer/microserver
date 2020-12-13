using Bytewizer.TinyCLR.Logging;

namespace Bytewizer.Playground.Hosting
{
    public class FooService : IFooService
    {
        private readonly ILogger _logger;

        public FooService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(typeof(FooService));
        }

        public void DoThing(int number)
        {
            _logger.LogInformation($"Doing the thing {number}");
        }
    }
}