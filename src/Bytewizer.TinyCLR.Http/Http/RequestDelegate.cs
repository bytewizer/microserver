namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// A function that can process a http request.
    /// </summary>
    /// <param name="context">The context for the request.</param>
    public delegate void RequestDelegate(HttpContext context);
}