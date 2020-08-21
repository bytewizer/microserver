namespace Bytewizer.TinyCLR.Sockets
{
    /// <summary>
    /// A function that can process a request.
    /// </summary>
    /// <param name="context">The <see cref="Context"/> for the request.</param>
    public delegate void RequestDelegate(IContext context);
}
