using System;

using Bytewizer.Extensions.DependencyInjection;

namespace Bytewizer.Extensions.Hosting.Internal
{
    internal interface IServiceFactoryAdapter
    {
        object CreateBuilder(IServiceCollection services);
        IServiceProvider CreateServiceProvider(object containerBuilder);
    }
}
