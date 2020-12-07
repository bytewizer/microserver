using System.Collections;

namespace System
{
    /// <summary>
    /// Defines a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
    /// </summary>
    public interface IServiceProvider
    {
        /// <summary>
        /// Gets the service object of type <paramref name="serviceType"/>.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <returns>
        /// A service object of type <paramref name="serviceType"/>. -or- null if there is no service object of type <paramref name="serviceType"/>.
        /// </returns>
        object GetService(Type serviceType);


        /// <summary>
        /// Get an enumeration of service objects of type <paramref name="serviceType"/>.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <returns>
        /// An enumeration of services of type <paramref name="serviceType"/>.
        /// </returns>
        IEnumerable GetServices(Type serviceType);
    }
}