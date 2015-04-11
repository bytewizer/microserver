using System.Collections;

namespace MicroServer.Net.Http
{
    /// <summary>
    /// Collection of cookies
    /// </summary>
    public interface IHttpCookieCollection : IEnumerable
    {
        /// <summary>
        /// Gets the count of cookies in the collection.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets the cookie of a given identifier (<c>null</c> if not existing).
        /// </summary>
        IHttpCookie this[string id] { get; }

        /// <summary>
        /// Add a cookie.
        /// </summary>
        /// <param name="cookie">Cookie to add</param>
        void Add(IHttpCookie cookie);

        /// <summary>
        /// Remove all cookies.
        /// </summary>
        void Clear();

        /// <summary>
        /// Remove a cookie from the collection.
        /// </summary>
        /// <param name="cookieName">Name of cookie.</param>
        void Remove(string cookieName);
    }
}