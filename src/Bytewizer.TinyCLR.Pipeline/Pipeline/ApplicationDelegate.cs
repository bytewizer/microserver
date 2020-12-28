using Bytewizer.TinyCLR.Pipeline.Builder;

namespace Bytewizer.TinyCLR.Pipeline
{
    /// <summary>
    /// Represents a function that can configure an application pipeline.
    /// </summary>
    public delegate void ApplicationDelegate(IApplicationBuilder builder);
}
