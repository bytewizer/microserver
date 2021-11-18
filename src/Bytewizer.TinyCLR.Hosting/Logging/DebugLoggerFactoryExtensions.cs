using Bytewizer.TinyCLR.DependencyInjection;

namespace Bytewizer.TinyCLR.Logging.Debug
{
    public static class DebugLoggerFactoryExtensions
    {
        /// <summary>
        /// Adds a debug logger to the factory.
        /// </summary>
        /// <param name="builder">The extension method argument.</param>
        public static ILoggingBuilder AddDebug(this ILoggingBuilder builder)
        {   
            builder.Services.AddSingleton(typeof(ILoggerProvider), typeof(DebugLoggerProvider));

            return builder;
        }
    }
}
