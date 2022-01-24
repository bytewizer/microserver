using System;
using System.Collections;

using Bytewizer.TinyCLR.Http.Internal;

namespace Bytewizer.TinyCLR.Http.Header
{
    /// <summary>
    /// A <see cref="HeaderDictionary"/> type for case-insensitive header values. 
    /// </summary>
    public class HeaderDictionary : IHeaderDictionary
    {
        /// <summary>
        /// The array list used to store the pairs.
        /// </summary>
        private readonly ArrayList _pairs;

        /// <summary>
        /// Thread synchronization.
        /// </summary>
        private readonly object _lock = new object();

        /// <summary>
        /// Initializes a new empty uninitialized instance of class.
        /// </summary>
        public HeaderDictionary()
        {
            _pairs = new ArrayList(10);
        }

        /// <inheritdoc/>
        public string this[string key]
        {
            get
            {
                var searchKey = key.ToLower();
                for (int i = 0; i < Count; i++)
                {
                    HeaderValue kvp = (HeaderValue)_pairs[i];
                    if (kvp.Key.ToLower() == searchKey)
                    {
                        return kvp.Value;
                    }
                }
                return null;
            }
            set
            {
                var searchKey = key.ToLower();
                for (int i = 0; i < Count; i++)
                {
                    HeaderValue kvp = (HeaderValue)_pairs[i];
                    if (kvp.Key.ToLower() == searchKey)
                    {
                        kvp.Value = value.Trim();
                        return;
                    }
                }

                lock (_lock)
                {
                    if (key.IndexOfAny(new[] { ':', '\r', '\n' }) != -1)
                    {
                        throw new FormatException("Header name may not contain colon, CR or LF; Name: " + value);
                    }

                    _pairs.Add(new HeaderValue(key, value.Trim()));
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
        public HeaderValue this[int index]
        {
            get
            {
                return (HeaderValue)_pairs[index];
            }
        }

        /// <inheritdoc/>
        public long ContentLength
        {
            get
            {
                var rawValue = this[HeaderNames.ContentLength];

                if (!string.IsNullOrEmpty(rawValue) &&
                    long.TryParse(rawValue.Trim(), out long value))
                {
                    return value;
                }

                return 0;
            }
            set
            {
                if (value <= -1)
                {
                    Remove(HeaderNames.ContentLength);
                }
                else
                {
                    this[HeaderNames.ContentLength] = value.ToString();
                }
            }
        }

        /// <inheritdoc/>
        public DateTime IfModifiedSince
        {
            get
            {
                var rawValue = this[HeaderNames.IfModifiedSince];

                if (!string.IsNullOrEmpty(rawValue) &&
                    DateTimeHelper.TryParse(rawValue.Trim(), out DateTime value))
                {
                    return value;
                }

                return DateTime.MinValue;
            }
            set
            {
                if (value <= DateTime.MinValue)
                {
                    Remove(HeaderNames.IfModifiedSince);
                }
                else
                {
                    this[HeaderNames.IfModifiedSince] = value.ToString();
                }
            }
        }

        /// <summary>
        /// Gets an <see cref="ICollection"/> containing the keys in the collection.
        /// </summary>
        public ICollection Keys
        {
            get
            {
                ArrayList list = new ArrayList();
                foreach (HeaderValue kvp in _pairs)
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
                ArrayList list = new ArrayList();
                foreach (HeaderValue kvp in _pairs)
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
            var searchKey = key.ToLower();
            for (int i = 0; i < Count; i++)
            {
                HeaderValue kvp = (HeaderValue)_pairs[i];
                if (kvp.Key.ToLower() == searchKey)
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
        public bool TryGetValue(string key, out string value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var searchKey = key.ToLower();
            for (int i = 0; i < Count; i++)
            {
                HeaderValue kvp = (HeaderValue)_pairs[i];
                if (kvp.Key.ToLower() == searchKey)
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
        /// <param name="key">The key to remove from the collection.</param>
        public void Remove(string key)
        {
            for (int i = 0; i < Count; i++)
            {
                HeaderValue nvp = (HeaderValue)_pairs[i];
                if (nvp.Key == key)
                {
                    _pairs.RemoveAt(i);
                    break;
                }
            }
        }

        /// <inheritdoc/>
        public void Add(string key, string value)
        {
            if (TryGetValue(key, out string existingValue))
            {
                // RFC 2965 - Origin servers SHOULD NOT fold multiple Set-Cookie header fields into a single header
                if (key.ToLower() == HeaderNames.SetCookie.ToLower())
                {
                    lock (_lock)
                    {
                        _pairs.Add(new HeaderValue(key, value.Trim()));
                    }
                }
                else
                {
                    Remove(key);
                    this[key] = $"{existingValue}, {value}";
                }
            }
            else
            {

                this[key] = value;
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
        /// The one-dimensional <see cref="Array"/> that is the destination of <see cref="HeaderValue"/> 
        /// objects copied from <see cref="ICollection"/>. The <see cref="Array"/> must have zero-based indexing.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the elements copied from the collection. The <see cref="Array"/> must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in array at which copying begins.</param>
        public void CopyTo(Array array, int index)
        {
            if (_pairs.Count > 0)
            {
                _pairs.CopyTo(array, index);
            }
        }

        /// <summary>
        /// Gets the number of key/value pairs contained in the collection.
        /// </summary>
        public int Count
        {
            get
            {
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
            return new HeaderEnumerator(this);
        }

        #endregion
    }
}