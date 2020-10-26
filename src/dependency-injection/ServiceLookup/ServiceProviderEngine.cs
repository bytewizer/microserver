using System;
using System.Reflection;

using System.Collections;

namespace Bytewizer.TinyCLR.DependencyInjection.ServiceLookup
{
    public class ServiceProviderEngine : IServiceProviderEngine, IServiceProvider, IDisposable
    {

        // Map of registered types
        private readonly Hashtable _resolvedServices = new Hashtable();
        
        public ServiceProviderEngine(IServiceCollection serviceDescriptors)
        {
            foreach (ServiceDescriptor serviceDescriptor in serviceDescriptors)
            {
                _resolvedServices[serviceDescriptor.ServiceType] = serviceDescriptor;
            }
        }

        public object GetService(Type serviceType)
        {
            ServiceDescriptor serviceDescriptor = (ServiceDescriptor)_resolvedServices[serviceType];

            if (serviceDescriptor == null)
            {
                return null;
            }

            if (serviceDescriptor.Lifetime == ServiceLifetime.Transient)
            {
                return Resolve(serviceDescriptor.ImplementationType);
            }

            //return SingletonCache.Instance().GetSingleton(serviceDescriptor.ImplementationType);

            return Resolve(serviceDescriptor.ImplementationType); // this should be cached.

        }

        public object Resolve(Type serviceType)
        {
            ConstructorInfo constructor = serviceType.GetConstructors()[0];
            ParameterInfo[] constructorParameters = constructor.GetParameters();
            if (constructorParameters.Length == 0)
            {
                return Activator.CreateInstance(serviceType);
            }

            ArrayList parameters = new ArrayList();
            foreach (ParameterInfo parameterInfo in constructorParameters)
            {
                parameters.Add(GetService(parameterInfo.ParameterType));
            }

            return constructor.Invoke(parameters.ToArray());
        }


        public void ValidateService(ServiceDescriptor descriptor)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
