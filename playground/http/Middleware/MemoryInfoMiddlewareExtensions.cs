using System;

using Bytewizer.TinyCLR.Http;
using Bytewizer.TinyCLR.Pipeline.Builder;

namespace Bytewizer.Playground.Http
{
    public static class MemoryInfoMiddlewareExtensions
    {
        public static void UseMemoryInfo(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.UseMiddleware(typeof(MemoryInfoMiddleware));
        }
    }
}
