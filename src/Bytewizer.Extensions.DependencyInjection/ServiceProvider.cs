using System;
using System.Reflection;
using System.Collections;

namespace Bytewizer.TinyCLR.DependencyInjection
{
    /// <summary>
    /// The default IServiceProvider.
    /// </summary>
    public sealed class ServiceProvider : IServiceProvider, IDisposable
    {
        internal Hashtable ResolvedServices { get; } = new Hashtable();

        internal ServiceProvider(IServiceCollection serviceDescriptors, ServiceProviderOptions options)
        {
            if (options.ValidateOnBuild)
            {
                ArrayList exceptions = null;
                foreach (ServiceDescriptor serviceDescriptor in serviceDescriptors)
                {
                    try
                    {
                        ValidateService(serviceDescriptor);
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

            foreach (ServiceDescriptor serviceDescriptor in serviceDescriptors)
            {
                lock (ResolvedServices)
                {
                    ResolvedServices[serviceDescriptor.ServiceType] = serviceDescriptor;
                }
            }
            ResolvedServices.Add(typeof(IServiceProvider), new ServiceDescriptor(typeof(IServiceProvider), this));
        }

        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <param name="serviceType">The type of the service to get.</param>
        /// <returns>The service that was produced.</returns>
        public object GetService(Type serviceType)
        {
            ServiceDescriptor serviceDescriptor = (ServiceDescriptor)ResolvedServices[serviceType];

            if (serviceDescriptor == null)
            {
                return null;
            }

            if (serviceDescriptor.Lifetime == ServiceLifetime.Singleton
                    & serviceDescriptor.ImplementationInstance != null)
            {
                return serviceDescriptor.ImplementationInstance;
            }
            else
            {
                var instance = Resolve(serviceDescriptor.ImplementationType);
                {
                    lock (ResolvedServices)
                    {
                        ResolvedServices[serviceDescriptor.ServiceType] = new ServiceDescriptor(serviceType, instance);
                    }
                }

                return instance;
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        private void ValidateService(ServiceDescriptor descriptor)
        {
            throw new NotImplementedException();
        }

        private object Resolve(Type implementationType)
        {
            ConstructorInfo constructor = implementationType.GetConstructors()[0];
            ParameterInfo[] constructorParameters = constructor.GetParameters();

            object instance;

            if (constructorParameters.Length == 0)
            {
                instance = Activator.CreateInstance(implementationType);
            }
            else
            {
                Type[] types = new Type[constructorParameters.Length];
                object[] parameters = new object[constructorParameters.Length];

                for (int i = 0; i < constructorParameters.Length; i++)
                {
                    var parameterType = constructorParameters[i].ParameterType;

                    var service = GetService(parameterType);
                    if (service == null)
                    {
                        throw new InvalidOperationException(
                            $"Unable to resolve service for type '{ parameterType }' while attempting to activate.");
                    }

                    parameters[i] = service;
                    types[i] = parameterType;
                }

                instance = Activator.CreateInstance(implementationType, types, parameters);
            }

            return instance;
        }
    }
}