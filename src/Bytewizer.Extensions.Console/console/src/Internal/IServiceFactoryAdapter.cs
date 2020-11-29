using System;

using Microsoft.Extensions.DependencyInjection;

namespace Bytewizer.Extensions.Console.Internal
{
    internal interface IServiceFactoryAdapter
    {
        object CreateBuilder(IServiceCollection services);
        IServiceProvider CreateServiceProvider(object containerBuilder);
    }
}
