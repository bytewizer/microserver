using Bytewizer.TinyCLR.DependencyInjection;

namespace Bytewizer.TinyCLR.Logging
{
    internal class LoggingBuilder : ILoggingBuilder
    {
        public LoggingBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }
    }
}
