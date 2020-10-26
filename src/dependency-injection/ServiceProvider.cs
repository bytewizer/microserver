using System;
using System.Collections;
using System.Reflection;
using Bytewizer.TinyCLR.DependencyInjection.ServiceLookup;

namespace Bytewizer.TinyCLR.DependencyInjection
{
    /// <summary>
    /// The default IServiceProvider.
    /// </summary>
    public sealed class ServiceProvider : IServiceProvider, IDisposable
    {
        //private readonly IServiceProviderEngine _engine;
        private readonly IServiceCollection _serviceDescriptors;
        
        private readonly Hashtable types = new Hashtable();

        internal ServiceProvider(IServiceCollection serviceDescriptors, ServiceProviderOptions options)
        {
            _serviceDescriptors = serviceDescriptors;

            foreach (ServiceDescriptor serviceDescriptor in serviceDescriptors)
            {
                types[serviceDescriptor.ServiceType] = serviceDescriptor.ImplementationType;
            }

            if (options.ValidateOnBuild)
            {
                ArrayList exceptions = null;
                foreach (ServiceDescriptor serviceDescriptor in serviceDescriptors)
                {
                    try
                    {
                        //_engine.ValidateService(serviceDescriptor);
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
            throw new NotImplementedException();
        }

        public object GetService(Type serviceType)
        {
            Type implementation = (Type)types[serviceType];

            ConstructorInfo constructor = implementation.GetConstructors()[0];
            ParameterInfo[] constructorParameters = constructor.GetParameters();
            if (constructorParameters.Length == 0)
            {
                return Activator.CreateInstance(implementation);
            }

            ArrayList parameters = new ArrayList();
            foreach (ParameterInfo parameterInfo in constructorParameters)
            {
                parameters.Add(GetService(parameterInfo.ParameterType));
            }

            return constructor.Invoke(parameters.ToArray());
        }

        //private static ConstructorInfo[] GetConstructors(Type type)
        //{
        //    BindingFlags bindingFlags = BindingFlags.CreateInstance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        //    MethodInfo[] methods = type.GetMethods(bindingFlags);

        //    ConstructorInfo[] constructors = new ConstructorInfo[methods.Length];
        //    for (int i = 0; i < constructors.Length; i++)
        //    {
        //        ParameterInfo[] constructorParameters = methods[i].GetParameters();

        //        Type[] parameters = new Type[constructorParameters.Length];
        //        for (int n = 0; n < parameters.Length; n++)
        //        {
        //            parameters[n] = constructorParameters[n].ParameterType;
        //        }

        //        constructors[i] = methods[i].DeclaringType.GetConstructor(parameters);
        //    }

        //    return constructors;
        //}
    }
}