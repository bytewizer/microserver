using System;

using Bytewizer.TinyCLR.Http;
using Bytewizer.TinyCLR.Sockets;

namespace Bytewizer.TinyCLR.WebServer
{
    public static class CustomMiddlewareExtensions
    {
        public static void UseCustomMiddleware(this ServerOptions app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.UseMiddleware(new CustomMiddleware());
        }
    }
}
