using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

namespace Bytewizer.Extensions.Console.Test
{
    public class Application : IApplication
    {
        public ILogger Logger { get; }

        public Application(ILogger<Application> logger)
        {
            Logger = logger;
        }

        public Task MainAsync(string[] args, CancellationToken cancellationToken)
        {
            Logger.LogCritical("Run MainAsync()");
            return Task.CompletedTask;
        }
    }
}
