using System;

using Bytewizer.TinyCLR.Http;
using Bytewizer.TinyCLR.Pipeline.Builder;

namespace Bytewizer.TinyCLR.TestHarness
{
    public static class TestHarnessExtensions
    {
        public static void UseTestHarness(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.UseMiddleware(typeof(RootResponse));
        }
    }
}
