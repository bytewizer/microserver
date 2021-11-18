using System;
using System.Threading;
using System.Collections;

using Bytewizer.TinyCLR.Logging;
using System.Text;

namespace Bytewizer.TinyCLR.Hosting.Internal
{
    internal class Host : IHost, IDisposable
    {
        private readonly ILogger _logger;

        private ArrayList _hostedServices;

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

        public IServiceProvider Services { get; }

        public void Start()
        {
            _logger.Starting();

            _hostedServices = (ArrayList)Services.GetServices(typeof(IHostedService));

            foreach (IHostedService hostedService in _hostedServices)
            {
                try
                {
                    // TODO: Thead exceptions are not passed back to main thread
                    hostedService.Start();
                    Stop();
                }
                catch (Exception ex)
                {
                    _logger.BackgroundServiceFaulted(ex);
                }
            }

           _logger.Started();
        }

        public void Stop()
        {
            _logger.Stopping();

            ArrayList exceptions = new ArrayList();
            foreach (IHostedService hostedService in _hostedServices)
            {
                try
                {
                    hostedService.Stop();
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Count > 0)
            {
                //var ex = new AggregateException("One or more hosted services failed to stop.", exceptions);
                //_logger.StoppedWithException(ex);
                //throw ex;
            }

            _logger.Stopped();
        }

        public void Dispose()
        {
        }
    }
}