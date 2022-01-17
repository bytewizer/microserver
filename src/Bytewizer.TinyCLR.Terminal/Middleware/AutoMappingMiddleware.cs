using Bytewizer.TinyCLR.Pipeline;
using Bytewizer.TinyCLR.Terminal.Features;

namespace Bytewizer.TinyCLR.Terminal
{
    /// <summary>
    /// Enables auto mapping of endpoints for command actions from assembly reflection.
    /// </summary>
    public class AutoMappingMiddleware : Middleware
    {
        /// <inheritdoc/>
        protected override void Invoke(ITerminalContext context, RequestDelegate next)
        {
            var endpointFeature = (EndpointFeature)context.Features.Get(typeof(EndpointFeature));
            endpointFeature.AutoMapping = true;

            next(context);
        }
    }
}