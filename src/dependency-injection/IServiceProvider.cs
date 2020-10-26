using System;

namespace Bytewizer.TinyCLR.DependencyInjection
{
    /// <summary>
    /// Defines a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
    /// </summary>
    public interface IServiceProvider
    {
        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <returns>
        /// A service object of type serviceType. -or- null if there is no service object of type serviceType.
        /// </returns>
        Object GetService(Type serviceType);
    }
}