using System;

namespace Bytewizer.TinyCLR.Logging
{
    /// <summary>
    /// An <see cref="ILoggerFactory"/> used to create instance of
    /// <see cref="NullLogger"/> that logs nothing.
    /// </summary>
    public class NullLoggerFactory : ILoggerFactory
    {
        /// <summary>
        /// Creates a new <see cref="NullLoggerFactory"/> instance.
        /// </summary>
        public NullLoggerFactory() { }

        /// <summary>
        /// Returns the shared instance of <see cref="NullLoggerFactory"/>.
        /// </summary>
        public static readonly NullLoggerFactory Instance = new NullLoggerFactory();

        /// <inheritdoc />
        /// <remarks>
        /// This returns a <see cref="NullLogger"/> instance which logs nothing.
        /// </remarks>
        public ILogger CreateLogger(string name)
        {
            return NullLogger.Instance;
        }

        /// <inheritdoc />
        /// <remarks>
        /// This method ignores the parameter and does nothing.
        /// </remarks>
        public void AddProvider(ILoggerProvider provider)
        {
        }

        /// <inheritdoc />
        public void Dispose()
        {
        }
    }
}
