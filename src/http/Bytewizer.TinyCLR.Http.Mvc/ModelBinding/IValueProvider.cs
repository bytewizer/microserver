using System.Collections;

namespace Bytewizer.TinyCLR.Http.Mvc.ModelBinding
{
    /// <summary>
    /// Defines the methods that are required for a value provider.
    /// </summary>
    public interface IValueProvider
    {
        /// <summary>
        /// Retrieves a value object using the specified key.
        /// </summary>
        /// <param name="key">The key of the value object to retrieve.</param>
        string Get(string key);

        /// <summary>
        /// Retrieves all value object.
        /// </summary>
        ICollection GetValues();

        /// <summary>
        /// Find all parameters which starts with the specified argument.
        /// </summary>
        /// <param name="prefix">Beginning of the field name</param>
        /// <returns>All matching parameters.</returns>
        IEnumerable Find(string prefix);
    }
}