using System;
using System.Collections;
using System.Text;

namespace MicroServer.Net.Http.Mvc.Resolver
{
    /// <summary>
    /// Implement the IServiceResolver interface to get support for IoC.
    /// </summary>
    public class ServiceResolver
    {
        private static ServiceResolver _instance = new ServiceResolver(new FactoryResolver());
        private IServiceResolver _resolver;

        private ServiceResolver(IServiceResolver serviceResolver)
        {
            _resolver = serviceResolver;

        }

        /// <summary>
        /// Gets current implementation
        /// </summary>
        public static ServiceResolver Current
        {
            get
            {
                if (_instance == null)
                    throw new InvalidOperationException("You must implement and assign service resolver.");
                return _instance;
            }
        }

        /// <summary>
        /// Assign your own implementation
        /// </summary>
        /// <param name="factory">Factory to use</param>
        public void Assign(IServiceResolver factory)
        {
            if (factory == null) throw new ArgumentNullException("factory");
            _instance = new ServiceResolver(factory);
        }

        public virtual object Resolve(Type type) 
        {
            return _resolver.Resolve(type);
        }
    }
}
