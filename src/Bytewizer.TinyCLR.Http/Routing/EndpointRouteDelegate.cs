namespace Bytewizer.TinyCLR.Http.Routing
{
    /// <summary>
    /// Represents a method to configure <see cref="EndpointMiddleware"/> specific features.
    /// </summary>
    /// <param name="configure">The <see cref="IEndpointRouteBuilder"/> configuration specific features.</param>
    public delegate void EndpointRouteDelegate(IEndpointRouteBuilder configure);
}
