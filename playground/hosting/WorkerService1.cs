using System;

using Bytewizer.TinyCLR.Hosting;
using Bytewizer.TinyCLR.Logging;


namespace Bytewizer.Playground.Hosting
{
    public class WorkerService1 : BackgroundService
    {
        public WorkerService1(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(typeof(WorkerService1));
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

                Logger.LogInformation($"[Worker Service 1] - #{count}");
            }
        }
    }
}
