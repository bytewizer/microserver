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
