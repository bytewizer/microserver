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
        private ArrayList _pairs;

        /// <summary>
        /// Thread synchronization.
        /// </summary>
        private readonly object _lock = new object();

        /// <summary>
        /// Initializes a new empty uninitialized instance of class.
        /// </summary>
        public HeaderDictionary() 
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
                if (_pairs == null)
                {
                    _pairs = new ArrayList();
                }

                var searchKey = key.ToLower();
                for (int i = 0; i < Count; i++)
                {
                    HeaderValue kvp = (HeaderValue)_pairs[i];
                    if (kvp.Key.ToLower() == searchKey)
                    {
                        kvp.Value = value;
                        return;
                    }
                }

                lock (_lock)
                {
                    _pairs.Add(new HeaderValue(key, value));
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

        /// <summary>
        /// Strongly typed access to the Content-Length header.
        /// </summary>
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

        /// <summary>
        /// Strongly typed access to the If-Modified-Since header.
        /// </summary>
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
                if (_pairs == null)
                {
                    return new ArrayList();
                }

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
                if (_pairs == null)
                {
                    return new ArrayList();
                }

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
            if (_pairs == null)
            {
                return false;
            }

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
            if (key == null) throw new ArgumentNullException(nameof(key));

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
            if (_pairs != null)
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

        /// <inheritdoc/>
        public string Connection
        {
            get { return this[HeaderNames.Connection]; }
            set { this[HeaderNames.Connection] = value; }
        }

        /// <inheritdoc/>
        public string Upgrade
        {
            get { return this[HeaderNames.Upgrade]; }
            set { this[HeaderNames.Upgrade] = value; }
        }

        /// <inheritdoc/>
        public string SecWebSocketAccept
        {
            get { return this[HeaderNames.SecWebSocketAccept]; }
            set { this[HeaderNames.SecWebSocketAccept] = value; }
        }

        /// <inheritdoc/>
        public string LastModified
        {
            get { return this[HeaderNames.LastModified]; }
            set { this[HeaderNames.LastModified] = value; }
        }

        /// <inheritdoc/>
        public string WWWAuthenticate
        {
            get { return this[HeaderNames.WWWAuthenticate]; }
            set { this[HeaderNames.WWWAuthenticate] = value; }
        }

        /// <inheritdoc/>
        public string Authorization
        {
            get { return this[HeaderNames.Authorization]; }
            set { this[HeaderNames.Authorization] = value; }
        }

        /// <inheritdoc/>  
        public string Accept
        {
            get { return this[HeaderNames.Accept]; }
            set { this[HeaderNames.Accept] = value; }
        }

        /// <inheritdoc/>
        public string AcceptCharset
        {
            get { return this[HeaderNames.AcceptCharset]; }
            set { this[HeaderNames.AcceptCharset] = value; }
        }

        /// <inheritdoc/>
        public string AcceptEncoding
        {
            get { return this[HeaderNames.AcceptEncoding]; }
            set { this[HeaderNames.AcceptEncoding] = value; }
        }

        /// <inheritdoc/>
        public string AcceptLanguage
        {
            get { return this[HeaderNames.AcceptLanguage]; }
            set { this[HeaderNames.AcceptLanguage] = value; }
        }

        /// <inheritdoc/>
        public string CacheControl
        {
            get { return this[HeaderNames.CacheControl]; }
            set { this[HeaderNames.CacheControl] = value; }
        }

        /// <inheritdoc/>
        public string ContentDisposition
        {
            get { return this[HeaderNames.ContentDisposition]; }
            set { this[HeaderNames.ContentDisposition] = value; }
        }

        /// <inheritdoc/>
        public string ContentRange
        {
            get { return this[HeaderNames.ContentRange]; }
            set { this[HeaderNames.ContentRange] = value; }
        }

        /// <inheritdoc/>
        public string ContentType
        {
            get { return this[HeaderNames.ContentType]; }
            set { this[HeaderNames.ContentType] = value; }
        }

        /// <inheritdoc/>
        public string Cookie
        {
            get { return this[HeaderNames.Cookie]; }
            set { this[HeaderNames.Cookie] = value; }
        }

        /// <inheritdoc/>
        public string Date
        {
            get { return this[HeaderNames.Date]; }
            set { this[HeaderNames.Date] = value; }
        }

        /// <inheritdoc/>
        public string Expires
        {
            get { return this[HeaderNames.Expires]; }
            set { this[HeaderNames.Expires] = value; }
        }

        /// <inheritdoc/>
        public string Host
        {
            get { return this[HeaderNames.Host]; }
            set { this[HeaderNames.Host] = value; }
        }

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