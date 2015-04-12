using MicroServer.Net.Http.Mvc;
using System.Collections;

namespace MicroServer.Net.Http.Mvc.Views
{
    /// <summary>
    /// Return data to a view.
    /// </summary>
    /// <remarks>
    /// Override GetHashCode and calculate the hash code for each
    /// parameter type. This is necessary to let the view engines
    /// cache strongly typed views (and be able to create new versions
    /// of a view if the types do not match).
    /// </remarks>
    public interface IViewData : IEnumerable
    {
        /// <summary>
        /// Checks if view data contains a parameter.
        /// </summary>
        /// <param name="key">Parameter key</param>
        /// <returns><c>true</c> if found; otherwise <c>false</c>.</returns>
        bool Contains(string key);

        /// <summary>
        /// Gets or sets a view data parameter.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        object this[string name] { get; set; }

        /// <summary>
        /// Try get a value from the dictionary.
        /// </summary>
        /// <param name="name">Name of parameter</param>
        /// <param name="value">Value if any.</param>
        /// <returns><c>true</c> if found; otherwise <c>false</c>.</returns>
        bool TryGetValue(string name, out object value);

        /// <summary>
        /// Gets dictionary count.
        /// </summary>
        int Count();
    }
}