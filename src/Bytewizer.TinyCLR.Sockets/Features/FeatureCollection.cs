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
        private ArrayList _pairs;

        /// <summary>
        /// Thread synchronization.
        /// </summary>
        private readonly object _lock = new object();

        /// <summary>
        /// Initializes a new empty uninitialized instance of class.
        /// </summary>
        public FeatureCollection() 
        { 
        }

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value to get or set.</param>
        /// <returns>
        /// The value associated with the specified key. If the specified key is not found,
        /// attempting to get it returns null, and attempting to set it creates a new element
        /// using the specified key.
        /// </returns>
        public object this[Type key]
        {
            get
            {
                if (_pairs == null)
                {
                    return null;
                }

                for (int i = 0; i < Count; i++)
                {
                    FeatureValue kvp = (FeatureValue)_pairs[i];
                    if (kvp.Key == key)
                    {
                        return kvp.Value;
                    }
                }
                return null;
            }
            set
            {
                if (_pairs == null)
                {
                    _pairs = new ArrayList();
                }

                for (int i = 0; i < Count; i++)
                {
                    FeatureValue kvp = (FeatureValue)_pairs[i];
                    if (kvp.Key == key)
                    {
                        kvp.Value = value;
                        return;
                    }
                }

                lock (_lock)
                {
                    _pairs.Add(new FeatureValue(key, value));
                }
            }
        }

        /// <summary>
        /// Gets the value associated with the specified index.
        /// </summary>
        /// <param name="index">The index whose value to get.</param>
        /// <returns>
        /// The value associated with the specified key. If the specified key is not found,
        /// attempting to get it returns null, and attempting to set it creates a new element
        /// using the specified key.
        /// </returns>
        public FeatureValue this[int index]
        {
            get
            {
                return (FeatureValue)_pairs[index];
            }
        }

        /// <summary>
        /// Retrieves the requested feature from the collection.
        /// </summary>
        /// <returns>The requested feature, or null if it is not present.</returns>
        public object Get(Type type)
        {
            return this[type];
        }

        /// <summary>
        /// Sets the given feature in the collection.
        /// </summary>
        /// <param name="type">The feature type.</param>
        /// <param name="instance">The feature value.</param>
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
            if (_pairs != null)
            {
                for (int i = 0; i < Count; i++)
                {
                    FeatureValue nvp = (FeatureValue)_pairs[i];
                    if (nvp.Key == key)
                    {
                        _pairs.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Removes all elements from the collection.
        /// </summary>
        public void Clear()
        {
            if (_pairs != null)
            {
                _pairs.Clear();
            }
        }

        #region ICollection Members

        /// <summary>
        /// The one-dimensional <see cref="Array"/> that is the destination of <see cref="FeatureValue"/> 
        /// objects copied from <see cref="ICollection"/>. The <see cref="Array"/> must have zero-based indexing.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the elements copied from the collection. The <see cref="Array"/> must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in array at which copying begins.</param>
        public void CopyTo(Array array, int index)
        {
            if (_pairs != null)
            {
                if (_pairs.Count > 0)
                {
                    _pairs.CopyTo(array, index);
                }
            }
        }

        /// <summary>
        /// Gets the number of key/value pairs contained in the collection.
        /// </summary>
        public int Count
        {
            get
            {
                if (_pairs == null)
                {
                    return 0;
                }

                return _pairs.Count;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the collection has a fixed size.
        /// </summary>
        public bool IsFixedSize
        {
            get
            {
                if (_pairs == null)
                {
                    return false;
                }

                return _pairs.IsFixedSize;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the collection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                if (_pairs == null)
                {
                    return false;
                }

                return _pairs.IsReadOnly;
            }
        }

        /// <summary>
        /// Gets a value indicating whether access to the collection is synchronized (thread safe).
        /// </summary>
        public bool IsSynchronized
        {
            get
            {
                if (_pairs == null)
                {
                    return false;
                }

                return _pairs.IsSynchronized;
            }
        }

        /// <summary>
        /// Gets an object that can be used to synchronize access to the collection.
        /// </summary>
        public object SyncRoot
        {
            get
            {
                if (_pairs == null)
                {
                    return new ArrayList().SyncRoot;
                }

                return _pairs.SyncRoot;
            }
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns <see cref="IEnumerator"/> that iterates through the collection.
        /// </summary>
        public IEnumerator GetEnumerator()
        {
            return new FeatureEnumerator(this);
        }

        #endregion
    }
}