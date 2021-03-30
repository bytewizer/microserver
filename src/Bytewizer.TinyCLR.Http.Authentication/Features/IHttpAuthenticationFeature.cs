namespace Bytewizer.TinyCLR.Http.Features
{
    /// <summary>
    /// A feature interface for authentication. Use <see cref="HttpContext.Features"/>
    /// to access an instance associated with the current request.
    /// </summary>
    public interface IHttpAuthenticationFeature
    {
        /// <summary>
        /// Gets or sets security information for the current HTTP request.
        /// </summary>
        IUser User { get; set; }
    }
}