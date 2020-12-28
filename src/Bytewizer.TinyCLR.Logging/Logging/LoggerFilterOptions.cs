namespace Bytewizer.TinyCLR.Logging
{
    /// <summary>
    /// The options for a LoggerFilter.
    /// </summary>
    public class LoggerFilterOptions
    {
        /// <summary>
        /// Creates a new <see cref="LoggerFilterOptions"/> instance.
        /// </summary>
        public LoggerFilterOptions() { }

        /// <summary>
        /// Gets or sets the minimum level of log messages if none of the rules match.
        /// </summary>
        public LogLevel MinLevel { get; set; }
    }
}
