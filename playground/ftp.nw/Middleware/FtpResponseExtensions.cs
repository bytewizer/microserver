using System;

using Bytewizer.TinyCLR.Pipeline.Builder;

namespace Bytewizer.Playground.Ftp
{
    public static class HttpResponseExtensions
    {
        public static void UseFtpResponse(this IApplicationBuilder app )
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.Use(new FtpResponse());
        }
    }
}
