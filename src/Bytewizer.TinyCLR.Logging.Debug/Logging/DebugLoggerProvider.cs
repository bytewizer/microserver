
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
        /// <param name="minLevel">The minimum level of log messages if none of the rules match.</param>
        public DebugLoggerProvider(LogLevel minLevel)
        {
            _minLevel = minLevel;
        }

        /// <inheritdoc />
        public ILogger CreateLogger(string name)
        {
            return new DebugLogger(name, _minLevel);
        }

        /// <summary>
        /// Pro-actively frees resources owned by this instance.
        /// </summary>
        public void Dispose()
        {
        }
    }
}
