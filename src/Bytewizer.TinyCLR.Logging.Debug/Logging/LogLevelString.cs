namespace Bytewizer.TinyCLR.Logging
{
    /// <summary>
    /// Defines logging severity level strings.
    /// </summary>
    public static class LogLevelString
    {
        /// <summary>
        /// Get the log level severity string.
        /// </summary>
        /// <param name="loglevel">The <see cref="LogLevel"/> to get.</param>
        public static string GetName(LogLevel loglevel)
        {
            switch (loglevel)
            {
                case LogLevel.Trace:
                    return "Trace";
                case LogLevel.Debug:
                    return "Debug";
                case LogLevel.Information:
                    return "Information";
                case LogLevel.Warning:
                    return "Warning";
                case LogLevel.Error:
                    return "Error";
                case LogLevel.Critical:
                    return "Critical";
                default:
                    return "None";
            }
        }
    }
}