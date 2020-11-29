using System.Collections;

namespace Bytewizer.TinyCLR.Http
{
    public interface IFormCollection
    {
        /// <summary>
        /// Gets the number of elements contained in the <see cref="IFormCollection" />.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets an <see cref="ICollection{T}" /> containing the keys of the
        /// <see cref="IFormCollection" />.
        /// </summary>
        ICollection Keys { get; }

        /// <summary>
        /// Determines whether the <see cref="IFormCollection" /> contains an element
        /// with the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="IFormCollection" />.</param>
        bool ContainsKey(string key);

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">The key of the value to get. When this method returns, the value associated with the specified key, if the
        /// key is found; otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.
        /// </param>

        bool TryGetValue(string key, out string[] value);

        /// <summary>
        /// Gets the value with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        string[] this[string key] { get; }

        /// <summary>
        /// The file collection sent with the request.
        /// </summary>
        IFormFileCollection Files { get; }
    }
}
