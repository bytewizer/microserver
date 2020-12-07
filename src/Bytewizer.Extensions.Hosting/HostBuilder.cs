using System;
using System.Collections;

using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.DependencyInjection;
using Bytewizer.TinyCLR.Logging.Debug;

using Bytewizer.Extensions.Configuration;

namespace Bytewizer.TinyCLR.Hosting
{
    public class HostBuilder : IHostBuilder
    {
        private readonly ArrayList _configureHostConfigActions = new ArrayList();
        private readonly ArrayList _configureServicesActions = new ArrayList();
        private HostBuilderContext _hostBuilderContext;
        private IServiceProvider _appServices;
        private bool _hostBuilt;

        private readonly DefaultServiceProviderFactory _serviceProviderFactory = new DefaultServiceProviderFactory();

        /// <summary>
        /// A central location for sharing state between components during the host building process.
        /// </summary>
        public IDictionary Properties { get; } = new Hashtable();

        /// <summary>
        /// Set up the configuration for the builder itself. This will be used to initialize the <see cref="IHostEnvironment"/>
        /// for use later in the build process. This can be called multiple times and the results will be additive.
        /// </summary>
        /// <param name="configureDelegate">The delegate for configuring the <see cref="IConfigurationBuilder"/> that will be used
        /// to construct the <see cref="IConfiguration"/> for the host.</param>
        /// <returns>The same instance of the <see cref="IHostBuilder"/> for chaining.</returns>
        public IHostBuilder ConfigureHostConfiguration(HostConfigAction configureDelegate)
        {
            _configureHostConfigActions.Add(configureDelegate ?? throw new ArgumentNullException(nameof(configureDelegate)));
            return this;
        }

        /// <summary>
        /// Adds services to the container. This can be called multiple times and the results will be additive.
        /// </summary>
        /// <param name="configureDelegate">The delegate for configuring the <see cref="IConfigurationBuilder"/> that will be used
        /// to construct the <see cref="IConfiguration"/> for the host.</param>
        /// <returns>The same instance of the <see cref="IHostBuilder"/> for chaining.</returns>
        public IHostBuilder ConfigureServices(ServiceContextDelegate configureDelegate)
        {
            _configureServicesActions.Add(configureDelegate ?? throw new ArgumentNullException(nameof(configureDelegate)));
            return this;
        }

        /// <summary>
        /// Run the given actions to initialize the host. This can only be called once.
        /// </summary>
        /// <returns>An initialized <see cref="IHost"/></returns>
        public IHost Build()
        {
            if (_hostBuilt)
            {
                throw new InvalidOperationException("Build can only be called once");
            }
            _hostBuilt = true;

            // Create host builder context
            _hostBuilderContext = new HostBuilderContext(Properties);

            // Create service provider
            var services = new ServiceCollection();

            services.AddSingleton(typeof(IHost), typeof(Internal.Host));
            services.AddSingleton(typeof(IHostBuilder), _hostBuilderContext);
            services.AddLogging();
            
            foreach (ServiceContextDelegate configureServicesAction in _configureServicesActions)
            {
                configureServicesAction(_hostBuilderContext, services);
            }

            IServiceCollection containerBuilder = _serviceProviderFactory.CreateBuilder(services);

            _appServices = _serviceProviderFactory.CreateServiceProvider(containerBuilder);

            if (_appServices == null)
            {
                throw new InvalidOperationException($"The IServiceProviderFactory returned a null IServiceProvider.");
            }

            // Create logging provider TODO: Why hard code this?
            var loggerFactory = (LoggerFactory)_appServices.GetService(typeof(ILoggerFactory));
            //var loggerprovider = (ILoggerProvider)_appServices.GetService(typeof(ILoggerProvider));
            //loggerFactory.AddProvider(loggerprovider);
            loggerFactory.AddProvider(new DebugLoggerProvider(LogLevel.Trace));

            return (Internal.Host)_appServices.GetService(typeof(IHost));
        }
    }
}
