using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Bytewizer.Extensions.Console.Internal
{
    internal class ConsoleApplication : IConsoleApplication
    {
        private readonly ILogger<ConsoleApplication> _logger;

        public ConsoleApplication(IServiceProvider services, ILogger<ConsoleApplication> logger)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// The programs configured services.
        /// </summary>
        public IServiceProvider Services { get; }

        public async Task RunAsync(string[] args, CancellationToken cancellationToken = default)
        {
            var applications = Services.GetServices<IApplication>();
            
            if (applications == null || applications.Count() <= 0) 
                throw new ArgumentNullException("No services were registered as an IApplication.", nameof(applications));
           
            foreach (var application in applications)
            {
                await application.MainAsync(args, cancellationToken);
            }
        }

        public void Dispose()
        {
            (Services as IDisposable)?.Dispose();
        }
    }
}
