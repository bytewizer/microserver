using System;

namespace Bytewizer.TinyCLR.Hosting
{
    /// <summary>
    /// Extension methods for configuring the <see cref="IHostBuilder" />.
    /// </summary>
    public static class GenericHostBuilderExtensions
    {
        /// <summary>
        /// Configures a <see cref="IHostBuilder" /> with defaults for hosting a web app. This should be called
        /// before application specific configuration to avoid it overwriting provided services, configuration sources,
        /// environments, content root, etc.
        /// </summary>
        /// <param name="builder">The <see cref="IHostBuilder" /> instance to configure.</param>
        /// <param name="configure">The configure callback</param>
        /// <returns>A reference to the <paramref name="builder"/> after the operation has completed.</returns>
        public static IHostBuilder ConfigureWebHostDefaults(this IHostBuilder builder, ConfigureWebHostAction configure)
        {
            if (configure is null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            //return builder.ConfigureWebHost(webHostBuilder =>
            //{
            //    WebHost.ConfigureWebDefaults(webHostBuilder);

            //    configure(webHostBuilder);
            //});

            //return builder.ConfigureWebHostDefaults(configure);
            return builder;
        }

        public delegate void ConfigureWebHostAction(IWebHostBuilder configure);

    }
}