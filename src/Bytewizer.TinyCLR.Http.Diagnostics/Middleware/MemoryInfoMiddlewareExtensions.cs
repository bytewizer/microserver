using System;


using Bytewizer.TinyCLR.Pipeline.Builder;

namespace Bytewizer.Playground.Sockets
{
    public static class MemoryInfoMiddlewareExtensions
    {
        public static void UseMemoryInfo(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.Use(new MemoryInfoMiddleware());
        }
    }
}
