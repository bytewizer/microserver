namespace Bytewizer.TinyCLR.Http.Features
{
    /// <summary>
    /// A feature interface for authentication. Use <see cref="HttpContext.Features"/>
    /// to access an instance associated with the current request.
    /// </summary>
    public interface IHttpCookiesFeature
    {
        /// <summary>
        /// Gets the collection of Cookies for this request.
        /// </summary>
        ICookieCollection Cookies { get; set; }

        /// <summary>
        /// Gets an object that can be used to manage cookies for this response.
        /// </summary>
        IResponseCookies ResponseCookies { get; }
    }
}