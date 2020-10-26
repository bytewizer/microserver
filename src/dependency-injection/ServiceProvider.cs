using System;
using System.Collections;

using Bytewizer.TinyCLR.DependencyInjection.ServiceLookup;

namespace Bytewizer.TinyCLR.DependencyInjection
{
    /// <summary>
    /// The default IServiceProvider.
    /// </summary>
    public sealed class ServiceProvider : IServiceProvider, IDisposable
    {
        private readonly IServiceProviderEngine _engine;

        internal ServiceProvider(IServiceCollection serviceDescriptors, IServiceProviderEngine engine, ServiceProviderOptions options)
        {
            _engine = engine;
            
            if (options.ValidateOnBuild)
            {
                ArrayList exceptions = null;
                foreach (ServiceDescriptor serviceDescriptor in serviceDescriptors)
                {
                    try
                    {
                        _engine.ValidateService(serviceDescriptor);
                    }
                    catch (Exception e)
                    {
                        exceptions = exceptions ?? new ArrayList();
                        exceptions.Add(e);
                    }
                }

                if (exceptions != null)
                {
                    //throw new AggregateException("Some services are not able to be constructed", exceptions.ToArray());
                }
            }
        }

        public void Dispose()
        {
            _engine.Dispose();
        }

        public object GetService(Type serviceType)
        {
            return _engine.GetService(serviceType);
        }
    }
}