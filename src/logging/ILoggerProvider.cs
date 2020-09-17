using System;

namespace Bytewizer.TinyCLR.Logging
{
    /// <summary>
    /// Represents a type that can create instances of <see cref="ILogger"/>.
    /// </summary>
    public interface ILoggerProvider : IDisposable
    {
        /// <summary>
        /// Creates a new <see cref="ILogger"/> instance.
        /// </summary>
        /// <param name="categoryName">The category name for messages produced by the logger.</param>
        ILogger CreateLogger(string categoryName);
    }
}
