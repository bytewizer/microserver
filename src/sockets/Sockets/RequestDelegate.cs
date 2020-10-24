namespace Bytewizer.TinyCLR.Sockets
{
    /// <summary>
    /// A function that can process an request.
    /// </summary>
    /// <param name="context">The context for the request.</param>
    public delegate void RequestDelegate(IContext context);
}
