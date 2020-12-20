using System;
using System.Collections;

namespace Bytewizer.TinyCLR.Http.Features
{
    /// <summary>
    /// An <see cref="FeatureCollection"/> type for key values.
    /// </summary>
    public class FeatureCollection : ICollection, IEnumerable, IFeatureCollection
    {
        /// <summary>
        /// The array list used to store the pairs.
        /// </summary>
        private ArrayList _pairs;

        /// <summary>
        /// Initializes a new empty uninitialized instance of class.
        /// </summary>
        public FeatureCollection() { }

        /// <summary>
        /// Initializes a new, empty instance of the class.
        /// <param name="capacity">The number of elements that the new list can initially store.</param>
        /// </summary>
        public FeatureCollection(int capacity)
        {
            _pairs = new ArrayList
            {
                Capacity = capacity
            };
        }

        /// <summary>
        ///  Initializes a new, empty instance of the class using the specified <see cref="ArrayList"/>.
        /// </summary>
        public FeatureCollection(ArrayList pairs)
        {
            _pairs = pairs;
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
                _pairs.Add(new FeatureValue(key, value));
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
        /// <param name="instance">The feature value.</param>
        public void Set(Type type, object instance)
        {

            this[type] = instance;
        }

        /// <summary>
        /// Gets an <see cref="ICollection"/> containing the keys in the collection.
        /// </summary>
        public ICollection Keys
        {
            get
            {
                if (_pairs == null)
                {
                    return new ArrayList();
                }

                ArrayList list = new ArrayList();
                foreach (FeatureValue kvp in _pairs)
                {
                    list.Add(kvp.Key);
                }
                return list;
            }
        }

        /// <summary>
        /// Gets an <see cref="ICollection"/> containing the values in the collection.
        /// </summary>
        public ICollection Values
        {
            get
            {
                if (_pairs == null)
                {
                    return new ArrayList();
                }

                ArrayList list = new ArrayList();
                foreach (FeatureValue kvp in _pairs)
                {
                    list.Add(kvp.Value);
                }
                return list;
            }
        }

        /// <summary>
        /// Adds the specified element to the collection.
        /// </summary>
        /// <param name="key">The key to use as the key of the element to add.</param>
        /// <param name="value">The value of the rule to add.</param>
        public void Add(Type key, string value)
        {
            if (_pairs == null)
            {
                _pairs = new ArrayList();
            }

            this[key] = value.Trim();
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

        /// <summary>
        /// Determines whether the collection contains a specific key.
        /// </summary>
        /// <param name="key">The key to locate in the collection.</param>
        public bool ContainsKey(Type key)
        {
            if (_pairs == null)
            {
                return false;
            }

            for (int i = 0; i < Count; i++)
            {
                FeatureValue kvp = (FeatureValue)_pairs[i];
                if (kvp.Key == key)
                {
                    return true;
                }
            }

            return false;
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
        public bool TryGetValue(Type key, out object value)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            for (int i = 0; i < Count; i++)
            {
                FeatureValue kvp = (FeatureValue)_pairs[i];
                if (kvp.Key == key)
                {
                    value = kvp.Value;
                    return true;
                }
            }

            value = default;
            return false;
        }

        /// <summary>
        /// Removes the element with the specified key from the collection.
        /// </summary>
        /// <param name="item">The <see cref="FeatureValue"/> to remove from the collection.</param>
        public void Remove(FeatureValue item)
        {
            if (_pairs != null)
            {
                _pairs.Remove(item.Key);
            }
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

        #region ICollection Members

        /// <summary>
        /// The one-dimensional array of type <see cref="FeatureValue"/> that is the destination of <see cref="FeatureValue"/> 
        /// objects copied from <see cref="ICollection"/>. The array must have zero-based indexing.
        /// </summary>
        /// <param name="array">The one-dimensional array of <see cref="FeatureValue"/> that is the destination of the elements copied from the collection. The <see cref="Array"/> must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in array at which copying begins.</param>
		public void CopyTo(FeatureValue[] array, int index)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (!(array is FeatureValue[] typedArray))
                throw new InvalidCastException(nameof(array));

            if (index < 0 || (typedArray.Length - index) < Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            if (_pairs != null)
            {
                foreach (Type key in Keys)
                {
                    typedArray[index++] = new FeatureValue(key, this[key]);
                }
            }
        }

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