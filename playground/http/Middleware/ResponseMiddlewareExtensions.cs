using System;

using Bytewizer.TinyCLR.Http;
using Bytewizer.TinyCLR.Pipeline.Builder;

namespace Bytewizer.Playground.Http
{
    public static class ResponseMiddlewareExtensions
    {
        public static void UseResponseMiddleware(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.SetProperty("key", "custom object/setting");
            app.UseMiddleware(typeof(ResponseMiddleware));
        }
    }
}
