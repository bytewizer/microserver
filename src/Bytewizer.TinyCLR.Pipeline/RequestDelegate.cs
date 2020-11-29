namespace Bytewizer.TinyCLR.Pipeline
{
    /// <summary>
    /// A function that can process a request.
    /// </summary>
    /// <param name="context">The context for the request.</param>
    public delegate void RequestDelegate(IContext context);
}