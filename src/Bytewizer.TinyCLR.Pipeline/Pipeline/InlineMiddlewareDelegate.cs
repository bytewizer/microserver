namespace Bytewizer.TinyCLR.Pipeline
{
    /// <summary>
    /// Represents a function that can process an inline pipeline middleware.
    /// </summary>
    public delegate void InlineMiddlewareDelegate(IContext context, RequestDelegate next);
}