using System;

namespace Bytewizer.TinyCLR.Logging
{
    /// <summary>
    /// Minimalistic logger that does nothing.
    /// </summary>
    public class NullLogger : ILogger
    {
        /// <summary>
        /// Returns the shared instance of <see cref="NullLogger"/>.
        /// </summary>
        public static NullLogger Instance { get; } = new NullLogger();
        private NullLogger()
        {
        }

        /// <inheritdoc />
        public bool IsEnabled(LogLevel logLevel)
        {
            return false;
        }

        /// <inheritdoc />
        public void Log(LogLevel logLevel, EventId eventId, object state, Exception exception)
        {
        }
    }
}
