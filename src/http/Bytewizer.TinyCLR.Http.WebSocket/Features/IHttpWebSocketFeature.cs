namespace Bytewizer.TinyCLR.Http.Features
{
    /// <summary>
    /// A feature interface for websockets. Use <see cref="HttpContext.Features"/>
    /// to access an instance associated with the current request.
    /// </summary>
    public interface IHttpWebSocketFeature
    {
        /// <summary>
        /// Indicates if this is a WebSocket upgrade request.
        /// </summary>
        bool IsWebSocketRequest { get; }
    }
}