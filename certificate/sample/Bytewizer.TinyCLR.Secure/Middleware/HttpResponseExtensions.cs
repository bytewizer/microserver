using System;

using Bytewizer.TinyCLR.Pipeline.Builder;

namespace Bytewizer.TinyCLR.Secure
{
    public static class HttpResponseExtensions
    {
        public static void UseHttpResponse(this IApplicationBuilder app )
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.Use(new HttpResponse());
        }
    }
}
