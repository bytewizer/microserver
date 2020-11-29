using System;
using System.Collections;

using Bytewizer.TinyCLR.Logging;

namespace Bytewizer.TinyCLR.Hosting.Internal
{
    internal class Host : IHost, IDisposable
    {
        private readonly ILogger _logger;

        private readonly ArrayList _hostedServices = new ArrayList();

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

            var service = Services.GetService(typeof(IHostedService));
            _hostedServices.Add(service);

            foreach (IHostedService hostedService in _hostedServices)
            {
                hostedService.StartAsync();
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