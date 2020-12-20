using Bytewizer.TinyCLR.Http;
using Bytewizer.TinyCLR.Sockets;
using Bytewizer.TinyCLR.DependencyInjection;

namespace Bytewizer.TinyCLR.Hosting
{
    /// <summary>
    /// Contains extensions for an <see cref="IHostBuilder"/>.
    /// </summary>
    public static class WebHostBuilderExtensions
    {

        ///// <summary>
        ///// Specify the startup method to be used to configure the web application.
        ///// </summary>
        ///// <param name="hostBuilder">The <see cref="IWebHostBuilder"/> to configure.</param>
        ///// <param name="configureApp">The delegate that configures the <see cref="IApplicationBuilder"/>.</param>
        ///// <returns>The <see cref="IWebHostBuilder"/>.</returns>
        //public static IWebHostBuilder Configure(this IWebHostBuilder hostBuilder, Action<IApplicationBuilder> configureApp)
        //{
        //    return hostBuilder.Configure((_, app) => configureApp(app), configureApp.GetMethodInfo().DeclaringType!.Assembly.GetName().Name!);
        //}

        ///// <summary>
        ///// Adds a delegate for configuring the provided <see cref="ILoggingBuilder"/>. This may be called multiple times.
        ///// </summary>
        ///// <param name="hostBuilder">The <see cref="IWebHostBuilder" /> to configure.</param>
        ///// <param name="configureLogging">The delegate that configures the <see cref="ILoggingBuilder"/>.</param>
        ///// <returns>The <see cref="IWebHostBuilder"/>.</returns>
        //public static IWebHostBuilder ConfigureLogging(this IWebHostBuilder hostBuilder, LoggingAction configureLogging)
        //{
        //    return hostBuilder.ConfigureServices(collection => collection.AddLogging(configureLogging));
        //}





        /// <summary>
        /// Adds and configures a web application.
        /// </summary>
        public static IHostBuilder ConfigureWebHostDefaults(this IHostBuilder builder)
        {
            var server = new HttpServer(options =>
            {
                options.UseMvc();
            });

            builder.ConfigureServices((context, services) => services.AddSingleton(typeof(IServer), server));
            builder.ConfigureServices((context, services) => services.AddHostedService(typeof(SocketHostService)));

            return builder;
        }

        /// <summary>
        /// Adds and configures a web application.
        /// </summary>
        public static IHostBuilder ConfigureWebHost(this IHostBuilder builder, ServerOptionsDelegate options)
        {
            var server = new HttpServer(options);
            builder.ConfigureServices((context, services) => services.AddSingleton(typeof(IServer), server));
            builder.ConfigureServices((context, services) => services.AddHostedService(typeof(SocketHostService)));

            return builder;
        }

        /// <summary>
        /// Adds and configures a socket server application.
        /// </summary>
        public static IHostBuilder ConfigureHost(this IHostBuilder builder, ServerOptionsDelegate options)
        {
            var server = new SocketServer(options);
            builder.ConfigureServices((context, services) => services.AddSingleton(typeof(IServer), server));
            builder.ConfigureServices((context, services) => services.AddHostedService(typeof(SocketHostService)));

            return builder;
        }
    }
}
