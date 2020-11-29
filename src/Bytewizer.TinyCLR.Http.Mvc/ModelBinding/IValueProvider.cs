using System.Collections;

namespace Bytewizer.TinyCLR.Http.Mvc.ModelBinding
{
    public interface IValueProvider
    {
        string Get(string name);
        ICollection GetValues();

        /// <summary>
        /// Find all parameters which starts with the specified argument.
        /// </summary>
        /// <param name="prefix">Beginning of the field name</param>
        /// <returns>All matching parameters.</returns>
        IEnumerable Find(string prefix);
    }
}
