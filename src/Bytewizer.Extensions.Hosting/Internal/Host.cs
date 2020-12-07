using System;

using Bytewizer.TinyCLR.Logging;

namespace Bytewizer.TinyCLR.Hosting.Internal
{
    internal class Host : IHost, IDisposable
    {
        private readonly ILogger _logger;

        public Host(IServiceProvider services, ILoggerFactory loggerFactory)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            Services = services;
            _logger = loggerFactory.CreateLogger(typeof(Host));
        }

        /// <summary>
        /// The programs configured services.
        /// </summary>
        public IServiceProvider Services { get; }

        public void StartAsync()
        {
            _logger.Starting();

            var hostedServices = Services.GetServices(typeof(IHostedService));
            foreach (IHostedService hostedService in hostedServices)
            {
                hostedService.Start();
            }

           _logger.Started();
        }

        public void StopAsync()
        {
            _logger.Stopping();      
            _logger.Stopped();
        }

        public void Dispose()
        {
        }
    }
}