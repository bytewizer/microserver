using System.Collections;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Represents a collection of query strings.
    /// </summary>
    public interface IQueryCollection : ICollection, IEnumerable
    {
        /// <summary>
        /// Gets the value with the specified key.
        /// </summary>
        /// <param name="key"> The key of the value to get.
        /// </param>
        string this[string key] { get; }

        /// <summary>
        /// Determines whether the <see cref="IQueryCollection" /> contains an element with the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="IQueryCollection" />.</param>
        bool ContainsKey(string key);

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">The key of the value to get. When this method returns, the value associated with the specified key, if the
        /// key is found; otherwise, the default value for the type of the value parameter.</param>
        bool TryGetValue(string key, out string value);

        /// <summary>
        /// Gets an <see cref="ArrayList" /> containing the keys of the <see cref="IQueryCollection" />.
        /// </summary>
        ICollection Keys { get; }


        /// <summary>
        /// Gets an <see cref="ArrayList"/> containing the values of the <see cref="IQueryCollection" />.
        /// </summary>
        ICollection Values { get; }
    }
}