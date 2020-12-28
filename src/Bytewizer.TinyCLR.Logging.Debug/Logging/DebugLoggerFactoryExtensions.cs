namespace Bytewizer.TinyCLR.Logging.Debug
{
    /// <summary>
    /// Extension methods for the <see cref="ILoggerFactory"/> class.
    /// </summary>
    public static class DebugLoggerFactoryExtensions
    {
        /// <summary>
        /// Adds a debug logger that is enabled for <see cref="LogLevel"/>.Information or higher.
        /// </summary>
        /// <param name="factory">The extension method argument.</param>
        public static ILoggerFactory AddDebug(this ILoggerFactory factory)
        {
            return AddDebug(factory, LogLevel.Information);
        }

        /// <summary>
        /// Adds a debug logger that is enabled as defined by the filter function.
        /// </summary>
        /// <param name="factory">The extension method argument.</param>
        /// <param name="minLevel">The minimum <see cref="LogLevel"/> to be logged</param>
        public static ILoggerFactory AddDebug(this ILoggerFactory factory, LogLevel minLevel)
        {
            factory.AddProvider(new DebugLoggerProvider(minLevel));
            return factory;
        }
    }
}