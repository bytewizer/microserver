using System;

using Bytewizer.TinyCLR.Pipeline.Builder;

namespace Bytewizer.TinyCLR.HelloWorld
{
    public static class DebugHeadersExtensions
    {
        public static void UseDebugHeaders(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.Use(new DebugHeaders());
        }
    }
}
