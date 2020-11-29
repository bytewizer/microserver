
using System;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Bytewizer.Extensions.Console
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Specify the startup type to be used by the console application.
        /// </summary>
        /// <param name="applicationBuilder">The <see cref="IApplicationBuilder"/> to configure.</param>
        /// <param name="startupType">The <see cref="Type"/> to be used.</param>
        /// <returns>The <see cref="IApplicationBuilder"/>.</returns>
        public static IApplicationBuilder UseStartup(this IApplicationBuilder applicationBuilder, Type startupType) 
        {
            return applicationBuilder.ConfigureServices((context, service) => { service.TryAddSingleton(startupType); });
        }

        /// <summary>
        /// Specify the startup type to be used by the console application.
        /// </summary>
        /// <typeparam name="TStartup">The type containing the startup methods for the application.</typeparam>
        /// <param name="applicationBuilder">The <see cref="IApplicationBuilder"/> to configure.</param>
        /// <returns>The <see cref="IApplicationBuilder"/>.</returns>
        public static IApplicationBuilder UseStartup<TStartup>(this IApplicationBuilder applicationBuilder)
            where TStartup : class, IApplication
        { 
            return applicationBuilder.ConfigureServices((context, service) => { service.TryAddEnumerable(ServiceDescriptor.Singleton<IApplication, TStartup>()); });
        }

        /// <summary>
        /// Specify the <see cref="IServiceProvider"/> to be the default one.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="IApplicationBuilder"/> to configure.</param>
        /// <param name="configure"></param>
        /// <returns>The <see cref="IApplicationBuilder"/>.</returns>
        public static IApplicationBuilder UseDefaultServiceProvider(this IApplicationBuilder hostBuilder, Action<ServiceProviderOptions> configure)
            => hostBuilder.UseDefaultServiceProvider(options => configure(options));

        /// <summary>
        /// Specify the <see cref="IServiceProvider"/> to be the default one.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="IApplicationBuilder"/> to configure.</param>
        /// <param name="configure">The delegate that configures the <see cref="IServiceProvider"/>.</param>
        /// <returns>The <see cref="IApplicationBuilder"/>.</returns>
        public static IApplicationBuilder UseDefaultServiceProvider(this IApplicationBuilder hostBuilder, Action<ApplicationBuilderContext, ServiceProviderOptions> configure)
        {
            return hostBuilder.UseServiceProviderFactory(context =>
            {
                var options = new ServiceProviderOptions();
                configure(context, options);
                return new DefaultServiceProviderFactory(options);
            });
        }

        /// <summary>
        /// Adds a delegate for configuring the provided <see cref="ILoggingBuilder"/>. This may be called multiple times.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="IApplicationBuilder" /> to configure.</param>
        /// <param name="configureLogging">The delegate that configures the <see cref="ILoggingBuilder"/>.</param>
        /// <returns>The same instance of the <see cref="IApplicationBuilder"/> for chaining.</returns>
        public static IApplicationBuilder ConfigureLogging(this IApplicationBuilder hostBuilder, Action<ApplicationBuilderContext, ILoggingBuilder> configureLogging)
        {
            return hostBuilder.ConfigureServices((context, collection) => collection.AddLogging(builder => configureLogging(context, builder)));
        }

        /// <summary>
        /// Adds a delegate for configuring the provided <see cref="ILoggingBuilder"/>. This may be called multiple times.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="IApplicationBuilder" /> to configure.</param>
        /// <param name="configureLogging">The delegate that configures the <see cref="ILoggingBuilder"/>.</param>
        /// <returns>The same instance of the <see cref="IApplicationBuilder"/> for chaining.</returns>
        public static IApplicationBuilder ConfigureLogging(this IApplicationBuilder hostBuilder, Action<ILoggingBuilder> configureLogging)
        {
            return hostBuilder.ConfigureServices((context, collection) => collection.AddLogging(builder => configureLogging(builder)));
        }

        /// <summary>
        /// Runs an application and block the calling thread until host shutdown.
        /// </summary>
        /// <param name="host">The <see cref="IConsoleApplication"/> to run.</param>
        public static void Run(this IConsoleApplication host)
        {
            host.RunAsync().GetAwaiter().GetResult();
        }
    }
}
