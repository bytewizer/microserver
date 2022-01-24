using System;

using Bytewizer.TinyCLR.Logging.Debug;

namespace Bytewizer.TinyCLR.Hosting
{
    /// <summary>
    /// Provides convenience methods for creating instances of <see cref="IHostBuilder"/> with pre-configured defaults.
    /// </summary>
    public static class Host
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HostBuilder"/> class with pre-configured defaults.
        /// </summary>
        /// <returns>The initialized <see cref="IHostBuilder"/>.</returns>
        public static IHostBuilder CreateDefaultBuilder()
        {
            var builder = new HostBuilder();

            builder.ConfigureLogging((context, logging) =>
             {
                 logging.AddDebug();
             });

             return builder;
        }
    }
}
