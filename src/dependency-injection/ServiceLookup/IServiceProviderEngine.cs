using System;

namespace Bytewizer.TinyCLR.DependencyInjection.ServiceLookup
{
    internal interface IServiceProviderEngine : IServiceProvider, IDisposable
    {
        void ValidateService(ServiceDescriptor descriptor);
    }
}
