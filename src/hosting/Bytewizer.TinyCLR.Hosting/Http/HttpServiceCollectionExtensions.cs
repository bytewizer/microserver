using System;
using Bytewizer.TinyCLR.Hosting;
using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Sockets;

namespace Bytewizer.TinyCLR.DependencyInjection
{
    /// <summary>
    /// Extension methods for setting up logging services in an <see cref="IServiceCollection" />.
    /// </summary>
    public static class HttpServiceCollectionExtensions
    {

        /// <summary>
        /// Adds logging services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configure">The <see cref="ServerOptionsDelegate"/> configuration delegate.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddHttpServer(this IServiceCollection services, ServerOptionsDelegate configure)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            //configure(new WebHostBuilder(services));

            return services;
        }
    }
}