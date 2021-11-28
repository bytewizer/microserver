using System;
using System.Collections;

namespace Bytewizer.TinyCLR.Features
{
    /// <summary>
    /// An <see cref="FeatureCollection"/> type for key values.
    /// </summary>
    public class FeatureCollection : IFeatureCollection
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
        public FeatureCollection()
        {
            _pairs = new Hashtable();
        }

        /// <inheritdoc/>
        public object this[Type key]
        {
            get
            {
                FeatureValue kvp = (FeatureValue)_pairs[key];
                if (_pairs.Contains(key))
                {
                    return kvp.Value;
                }

                return null;
            }
            set
            {
                FeatureValue kvp = (FeatureValue)_pairs[key];
                if (_pairs.Contains(key))
                {
                    kvp.Value = value;
                    return;
                }

                lock (_lock)
                {
                    _pairs.Add(key, new FeatureValue(key, value));
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
        public FeatureValue this[object key]
        {
            get
            {
                return (FeatureValue)_pairs[key];
            }
        }

        /// <inheritdoc/>
        public object Get(Type type)
        {
            return this[type];
        }

        /// <inheritdoc/>
        public void Set(Type type, object instance)
        {
            this[type] = instance;
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