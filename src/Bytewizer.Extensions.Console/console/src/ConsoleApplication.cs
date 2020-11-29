using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace Bytewizer.Extensions.Console
{
    /// <summary>
    /// Provides convenience methods for creating instances of <see cref="IApplicationBuilder"/> with pre-configured defaults.
    /// </summary>
    public static class ConsoleApplication
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationBuilder"/> class with pre-configured defaults.
        /// </summary>
        /// <returns>The initialized <see cref="IApplicationBuilder"/>.</returns>
        public static IApplicationBuilder CreateDefaultBuilder() =>
            CreateDefaultBuilder(args: null);

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationBuilder"/> class with pre-configured defaults.
        /// </summary>
        /// <returns>The initialized <see cref="IApplicationBuilder"/>.</returns>
        public static IApplicationBuilder CreateDefaultBuilder(string[] args)
        {
            var builder = new ApplicationBuilder();

            builder.ConfigureHostConfiguration(config =>
            {
                if (args != null)
                {
                    config.AddCommandLine(args);
                }
            });

            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("settings.json", optional: true, reloadOnChange: true);
                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                config.AddEnvironmentVariables();

                if (args != null)
                {
                    config.AddCommandLine(args);
                }
            });

            builder.ConfigureLogging((context, logging) =>
            {
                logging.AddConfiguration(context.Configuration.GetSection("Logging"));
                logging.AddConsole();
                logging.AddDebug();
            });

            builder.UseDefaultServiceProvider((context, options) =>
            {
                options.ValidateScopes = false;
            });

            return builder;
        }
    }
}
