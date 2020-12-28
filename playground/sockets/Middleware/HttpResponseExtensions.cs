using System;

using Bytewizer.TinyCLR.Pipeline.Builder;

namespace Bytewizer.Playground.Sockets
{
    public static class HttpResponseExtensions
    {
        public static void UseHttpResponse(this IApplicationBuilder app )
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.SetProperty("key", "custom object/setting");
            app.Use(new HttpResponse());
        }
    }
}
