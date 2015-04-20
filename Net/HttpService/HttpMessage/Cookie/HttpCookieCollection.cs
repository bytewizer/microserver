using System;
using System.Collections;


namespace MicroServer.Net.Http.Messages
{
    /// <summary>
    /// A collection of HTTP cookies
    /// </summary>
    public class HttpCookieCollection : IHttpCookieCollection
    {
        private readonly Hashtable _items = new Hashtable();

        #region IHttpCookieCollection Members


        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        /// <summary>
        /// Adds the specified cookie.
        /// </summary>
        /// <param name="cookie">The cookie.</param>
        public void Add(IHttpCookie cookie)
        {
            if (cookie == null)
                throw new ArgumentNullException("cookie");

            _items.Add(cookie.Name, cookie);
        }

        /// <summary>
        /// Gets the count of cookies in the collection.
        /// </summary>
        public int Count
        {
            get { return _items.Count; }
        }

        /// <summary>
        /// Gets the cookie of a given identifier (<c>null</c> if not existing).
        /// </summary>
        public IHttpCookie this[string cookieName]
        {
            get
            {
                if (cookieName == null) throw new ArgumentNullException("cookieName");

                return _items.Contains(cookieName) ? (IHttpCookie)_items[cookieName] : null;
            }
        }

        /// <summary>
        /// Remove all cookies.
        /// </summary>
        public void Clear()
        {
            _items.Clear();
        }

        /// <summary>
        /// Remove a cookie from the collection.
        /// </summary>
        /// <param name="cookieName">Name of cookie.</param>
        public void Remove(string cookieName)
        {
            if (cookieName == null) throw new ArgumentNullException("cookieName");
            lock (_items)
            {
                if (!_items.Contains(cookieName))
                    return;

                _items.Remove(cookieName);
            }
        }

        #endregion
    }
}