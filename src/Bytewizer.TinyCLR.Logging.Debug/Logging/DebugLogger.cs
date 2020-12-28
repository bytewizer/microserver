using System;
using System.Text;
using System.Diagnostics;

namespace Bytewizer.TinyCLR.Logging.Debug
{
    /// <summary>
    /// A logger that writes messages in the debug output window only when a debugger is attached.
    /// </summary>
    internal partial class DebugLogger : ILogger
    {
        private readonly string _name;
        private readonly LogLevel _minLevel;

        /// <summary>
        /// Initializes a new instance of the <see cref="DebugLogger"/> class.
        /// </summary>
        public DebugLogger(string name)
            : this(name, LogLevel.Trace)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DebugLogger"/> class.
        /// </summary>
        /// <param name="name">The name of the logger.</param>
        /// <param name="minLevel">The minimum level of log messages to filter messages.</param>
        public DebugLogger(string name, LogLevel minLevel)
        {
            _name = string.IsNullOrEmpty(name) ? nameof(DebugLogger) : name;
            _minLevel = minLevel;
        }

        /// <inheritdoc />
        public bool IsEnabled(LogLevel logLevel)
        {
            // If the filter is null, everything is enabled
            // unless the debugger is not attached
            return Debugger.IsAttached &&
                logLevel != LogLevel.None &&
                LogFilter(logLevel);
        }

        private bool LogFilter(LogLevel logLevel)
        {
            if (_minLevel <= logLevel)
            {
                return true ;
            }

            return false;
        }

        /// <inheritdoc />
        public void Log(LogLevel logLevel, EventId eventId, object state, Exception exception)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            var message = state.ToString();

            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            var builder = new StringBuilder();

            builder.Append(LogLevelString.GetName(logLevel));
            builder.Append(": ");
            builder.Append(_name);
            builder.Append("[");
            builder.Append(eventId.Id);
            builder.Append("]");
            builder.Append(": ");
            builder.Append(message);

            if (exception != null)
            {
                builder.Append(": ");
                builder.Append(exception);
            }

            DebugWriteLine(builder.ToString());
        }
    }
}
