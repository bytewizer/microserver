using System;

namespace Bytewizer.TinyCLR.Logging
{
    internal readonly struct LoggerInformation
    {
        public LoggerInformation(ILoggerProvider provider, string category) 
            : this()
        {
            ProviderType = provider.GetType();
            Logger = provider.CreateLogger(category);
            Category = category;
        }

        public ILogger Logger { get; }

        public string Category { get; }

        public Type ProviderType { get; }
    }
}
