namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Represents the response Set-Cookie header.
    /// </summary>
    public interface IResponseCookies
    {
        /// <summary>
        /// Add a new cookie and value.
        /// </summary>
        /// <param name="key">Name of the new cookie.</param>
        /// <param name="value">Value of the new cookie.</param>
        void Append(string key, string value);

        /// <summary>
        /// Add a new cookie.
        /// </summary>
        /// <param name="key">Name of the new cookie.</param>
        /// <param name="value">Value of the new cookie.</param>
        /// <param name="maxAge">Cookie age in seconds until it expires (default is 86400)</param>
        /// <param name="path">Cookie path (default is "")</param>
        /// <param name="domain">Cookie domain (default is "")</param>
        /// <param name="secure">Cookie secure flag (default is true)</param>
        /// <param name="httpOnly">Cookie HTTP-only flag (default is false)</param>
        void Append(string key,
            string value,
            int maxAge = 86400,
            string path = "",
            string domain = "",
            bool secure = true,
            bool httpOnly = false);

        /// <summary>
        /// Sets an expired cookie.
        /// </summary>
        /// <param name="key">Name of the cookie to expire.</param>
        void Delete(string key);
    }
}