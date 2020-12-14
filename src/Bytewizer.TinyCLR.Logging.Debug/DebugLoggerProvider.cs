
namespace Bytewizer.TinyCLR.Logging.Debug
{
    /// <summary>
    /// The provider for the <see cref="DebugLogger"/>.
    /// </summary>
    public class DebugLoggerProvider : ILoggerProvider
    {
        private readonly LogLevel _minLevel;

        /// <summary>
        /// Initializes a new instance of the <see cref="DebugLoggerProvider"/> class that
        /// is enabled for <see cref = "LogLevel" /> Information or higher.
        /// </summary>
        public DebugLoggerProvider()
        {
            _minLevel = LogLevel.Information;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DebugLoggerProvider"/> class.
        /// </summary>
        /// <param name="filter">The function used to filter events based on the log level.</param>
        public DebugLoggerProvider(LogLevel minLevel)
        {
            _minLevel = minLevel;
        }

        /// <inheritdoc />
        public ILogger CreateLogger(string name)
        {
            return new DebugLogger(name, _minLevel);
        }

        public void Dispose()
        {
        }
    }
}
