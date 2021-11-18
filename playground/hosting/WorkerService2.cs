
using Bytewizer.TinyCLR.Hosting;
using Bytewizer.TinyCLR.Logging;

namespace Bytewizer.Playground.Hosting
{
    public class WorkerService2 : BackgroundService
    {
        public WorkerService2(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(typeof(WorkerService2));
        }

        public ILogger Logger { get; }

        protected override void ExecuteAsync()
        {
            long count;   
            for (count = 0; count < 100; count++)
            {
                if (IsCancellationRequested)
                {
                    return;
                }

                Logger.LogInformation($"[Worker Service 2] - #{count}");
            }
        }
    }
}
