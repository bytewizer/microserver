using System;

using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.DependencyInjection;

namespace Bytewizer.TinyCLR.Hosting
{
    //public delegate void HostConfigAction(IHostBuilder obj);
    //public delegate void AppConfigAction(HostBuilderContext obj1, IHostBuilder obj2);

    //public delegate void ConfigAction(IConfigurationBuilder obj);

    public delegate void ServiceAction(IServiceCollection obj);
    //public delegate void ServiceContextDelegate(HostBuilderContext obj1, IServiceCollection obj2);

    public delegate void ServiceDelegate(HostBuilderContext context, IServiceCollection serviceCollection);

    public delegate void LoggingAction(ILoggingBuilder obj);
    public delegate void LoggingContextDelegate(HostBuilderContext context, ILoggingBuilder builder);
}