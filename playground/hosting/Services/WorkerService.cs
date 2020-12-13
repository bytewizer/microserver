using System;
using System.Threading;
using Bytewizer.TinyCLR.Hosting;
using Bytewizer.TinyCLR.Logging;

namespace Bytewizer.Playground.Hosting
{
    public class WorkerService : BackgroundService
    {
        private readonly ILogger _logger;

        public WorkerService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(typeof(WorkerService));
        }

        protected override void ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"Worker running at: {DateTime.Now}");
                Thread.Sleep(1000);
            }
        }
    }
}
