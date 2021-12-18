using System;
using System.Collections;

namespace Bytewizer.TinyCLR.Telnet
{
    /// <summary>
    /// An <see cref="ArgumentCollection"/> type for key values.
    /// </summary>
    public class ArgumentCollection
    {
        /// <summary>
        /// The array list used to store the pairs.
        /// </summary>
        private readonly Hashtable _pairs;

        /// <summary>
        /// Thread synchronization.
        /// </summary>
        private readonly object _lock = new object();

        /// <summary>
        /// Initializes a new empty uninitialized instance of class.
        /// </summary>
        public ArgumentCollection()
        {
            _pairs = new Hashtable();
        }

        /// <summary>
        ///  Initializes a new empty instance of the class using the specified <see cref="Hashtable"/>.
        /// </summary>
        public ArgumentCollection(Hashtable pairs)
        {
            _pairs = pairs;
        }

        /// <inheritdoc/>
        public string this[string key]
        {
            get
            {
                ArgumentValue kvp = (ArgumentValue)_pairs[key];
                if (_pairs.Contains(key))
                {
                    return kvp.Value;
                }

                return null;
            }
            set
            {
                ArgumentValue kvp = (ArgumentValue)_pairs[key];
                if (_pairs.Contains(key))
                {
                    kvp.Value = value;
                    return;
                }

                lock (_lock)
                {
                    _pairs.Add(key, new ArgumentValue(key, value));
                }
            }
        }

        /// <summary>
        /// Gets the value associated with the specified index.
        /// </summary>
        /// <param name="key">The key whose value to get or set.</param>
        /// <returns>
        /// The value associated with the specified key. If the specified key is not found,
        /// attempting to get it returns null, and attempting to set it creates a new element
        /// using the specified key.
        /// </returns>
        public ArgumentValue this[object key]
        {
            get
            {
                return (ArgumentValue)_pairs[key];
            }
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the
        /// key is found; otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.
        /// </param>
        /// <returns>
        /// <c>true</c> if the object that implements collection contains an element with the specified key; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Specified key is <c>null</c></exception>
        public bool TryGetValue(string key, out string value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            ArgumentValue kvp = (ArgumentValue)_pairs[key];
            if (_pairs.Contains(key))
            {
                value = kvp.Value;
                return true;
            }

            value = default;
            return false;
        }

        /// <summary>
        /// Removes the element with the specified key from the collection.
        /// </summary>
        /// <param name="key">The key to remove from the collection.</param>
        public void Remove(Type key)
        {
            if (_pairs.Contains(key))
            {
                _pairs.Remove(key);
            }
        }

        /// <summary>
        /// Removes all elements from the collection.
        /// </summary>
        public void Clear()
        {
            _pairs.Clear();
        }
    }
}