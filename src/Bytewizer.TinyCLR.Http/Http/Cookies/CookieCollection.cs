using System;
using System.Collections;

namespace Bytewizer.TinyCLR.Http.Cookies
{
    /// <summary>
    /// An <see cref="CookieCollection"/> type for cookie values.
    /// </summary>
    public class CookieCollection : ICookieCollection
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
        public CookieCollection() 
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
        public string this[string key]
        {
            get
            {
                if (_pairs == null)
                {
                    return null;
                }

                for (int i = 0; i < Count; i++)
                {
                    CookieValue kvp = (CookieValue)_pairs[i];
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
                    CookieValue kvp = (CookieValue)_pairs[i];
                    if (kvp.Key == key)
                    {
                        kvp.Value = value;
                        return;
                    }
                }

                lock (_lock)
                {
                    _pairs.Add(new CookieValue(key, value));
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
        public CookieValue this[int index]
        {
            get 
            { 
                return ((CookieValue)_pairs[index]); 
            }
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
                    return default;
                }

                ArrayList list = new ArrayList();
                foreach (CookieValue kvp in _pairs)
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
                    return default;
                }

                var list = new ArrayList();
                foreach (CookieValue kvp in _pairs)
                {
                    list.Add(kvp.Value);
                }
                return list;
            }
        }

        /// <summary>
        /// Determines whether the collection contains a specific key.
        /// </summary>
        /// <param name="key">The key to locate in the collection.</param>
        public bool ContainsKey(string key)
        {
            if (_pairs == null)
            {
                return false;
            }

            return _pairs.Contains(key);
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
            if (key == null) throw new ArgumentNullException(nameof(key));

            var item = this[key];

            if (item == null)
            {
                value = default;
                return false;
            }

            value = item;
            return true;
        }

        /// <summary>
        /// Removes the element with the specified key from the collection.
        /// </summary>
        /// <param name="key">The key to remove from the collection.</param>
        public void Remove(string key)
        {
            if (_pairs != null)
            {
                for (int i = 0; i < Count; i++)
                {
                    CookieValue nvp = (CookieValue)_pairs[i];
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
        /// The one-dimensional <see cref="Array"/> that is the destination of <see cref="CookieValue"/> 
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
            return new CookieEnumerator(this);
        }

        #endregion
    }
}