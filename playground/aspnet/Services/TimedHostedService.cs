using System;
using System.Threading;

using Bytewizer.TinyCLR.Hosting;
using Bytewizer.TinyCLR.Logging;

namespace Bytewizer.Playground.AspNet
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger _logger;
        private Timer _timer;

        public TimedHostedService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(typeof(TimedHostedService)); 
        }

        public void Start()
        {
            _logger.LogInformation("Timed Hosted Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(5));
        }

        private void DoWork(object state)
        {
            var count = Interlocked.Increment(ref executionCount);

            _logger.LogInformation(
                $"Timed Hosted Service is working. Count: {count}");
        }

        public void Stop()
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");
            _timer.Change(Timeout.Infinite, 0);
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}