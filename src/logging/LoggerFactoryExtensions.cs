using System;

namespace Bytewizer.TinyCLR.Logging
{
    /// <summary>
    /// ILoggerFactory extension methods for common scenarios.
    /// </summary>
    public static class LoggerFactoryExtensions
    {
        /// <summary>
        /// Creates a new <see cref="ILogger"/> instance using the full name of the given <paramref name="type"/>.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="type">The type.</param>
        /// <return>The <see cref="ILogger"/> that was created.</return>
        public static ILogger CreateLogger(this ILoggerFactory factory, Type type)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return factory.CreateLogger(TypeNameHelper.GetTypeDisplayName(type));
        }
    }
}