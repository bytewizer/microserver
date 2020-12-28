using System.Collections;

namespace Bytewizer.TinyCLR.Logging
{
    /// <summary>
    /// Produces instances of <see cref="ILogger"/> classes based on the given providers.
    /// </summary>
    public class LoggerFactory : ILoggerFactory
    {
        private readonly Hashtable _loggers = new Hashtable();
        private readonly ArrayList _providerRegistrations = new ArrayList();

        private readonly LoggerFilterOptions _filterOptions;
        private readonly object _sync = new object();
        private bool _disposed;

        /// <summary>
        /// Creates a new <see cref="LoggerFactory"/> instance.
        /// </summary>
        public LoggerFactory()
            : this (new ArrayList(), new LoggerFilterOptions())
        {
        }

        /// <summary>
        /// Creates a new <see cref="LoggerFactory"/> instance.
        /// </summary>
        /// <param name="providers">The providers to use in producing <see cref="ILogger"/> instances.</param>
        /// <param name="filterOption">The filter option to use.</param>
        public LoggerFactory(IEnumerable providers, LoggerFilterOptions filterOption)
        {
            foreach (ILoggerProvider provider in providers)
            {
                _providerRegistrations.Add(provider);
            }

            _filterOptions = filterOption;
        }

        /// <summary>
        /// Adds the given provider to those used in creating <see cref="ILogger"/> instances.
        /// </summary>
        /// <param name="provider">The <see cref="ILoggerProvider"/> to add.</param>
        public void AddProvider(ILoggerProvider provider)
        {
            _providerRegistrations.Add(provider);
        }

        /// <summary>
        /// Creates an <see cref="ILogger"/> with the given <paramref name="categoryName"/>.
        /// </summary>
        /// <param name="categoryName">The category name for messages produced by the logger.</param>
        /// <returns>The <see cref="ILogger"/> that was created.</returns>
        public ILogger CreateLogger(string categoryName)
        {
            lock (_sync)
            {
                var logger = new Logger() {
                    Loggers = CreateLoggers(categoryName)
                };
                logger.MessageLoggers = CreateMessageLoggers(logger.Loggers);

                _loggers[categoryName] = logger;

                return logger;
            }
        }

        private MessageLogger[] CreateMessageLoggers(LoggerInformation[] loggers)
        {
            var messageLoggers = new MessageLogger[loggers.Length];
            
            var minLevel = _filterOptions.MinLevel;
            for (int i = 0; i < loggers.Length; i++)
            {
                if (minLevel > LogLevel.Critical)
                {
                    continue;
                }

                messageLoggers[i] = new MessageLogger(
                    loggers[i].Logger, loggers[i].Category, loggers[i].ProviderType.FullName, minLevel);
            }

            return messageLoggers;
        }

        private LoggerInformation[] CreateLoggers(string categoryName)
        {
            var loggers = new LoggerInformation[_providerRegistrations.Count];
            for (var i = 0; i < _providerRegistrations.Count; i++)
            {
                loggers[i] = new LoggerInformation((ILoggerProvider)_providerRegistrations[i], categoryName);
            }
            return loggers;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                foreach (ILoggerProvider registration in _providerRegistrations)
                {
                    try
                    {
                        registration.Dispose();
                    }
                    catch
                    {
                        // Swallow exceptions on dispose
                    }
                }
            }
        }
    }
}
