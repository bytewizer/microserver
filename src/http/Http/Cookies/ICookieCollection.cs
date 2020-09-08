using System.Collections;

namespace Bytewizer.TinyCLR.Http
{
    public interface ICookieCollection
    {
        /// <summary>
        /// Gets the number of elements contained in the <see cref="ICookieCollection" />.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets an <see cref="ICollection" /> containing the keys of the <see cref="ICookieCollection" />.
        /// </summary>
        ICollection Keys { get; }

        /// <summary>
        /// Determines whether the <see cref="ICookieCollection" /> contains an element with the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="ICookieCollection" />.</param>
        bool ContainsKey(string key);

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">
        /// The key of the value to get. When this method returns, the value associated with the specified key, if the
        /// key is found; otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.
        /// </param>
        bool TryGetValue(string key, out string value);

        /// <summary>
        /// Gets the value with the specified key.
        /// </summary>
        /// <param name="key"> The key of the value to get.</param>
        string this[string key] { get; }
    }
}
