using System;

using Bytewizer.TinyCLR.Pipeline.Builder;

namespace Bytewizer.TinyCLR.Netbios
{
    public static class NetBiosExtensions
    {
        public static void UseNetBios(this IApplicationBuilder app, string name, byte[] address)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.Use(new NetBios(name, address));
        }
    }
}
