
using Bytewizer.TinyCLR.Hosting;
using Bytewizer.TinyCLR.Logging;

namespace Bytewizer.Playground.Hosting
{
    public class MainService : BackgroundService
    {
        private readonly IBarService _barService;

        public MainService(ILoggerFactory loggerFactory, IBarService barService)
        {
            Logger = loggerFactory.CreateLogger(typeof(MainService));
            _barService = barService;
        }

        public ILogger Logger { get; }

        protected override void ExecuteAsync()
        {
            Logger.LogInformation("Hit ExecuteAsync()");

            //do the actual work here
            _barService.DoSomeRealWork();
        }
    }
}
