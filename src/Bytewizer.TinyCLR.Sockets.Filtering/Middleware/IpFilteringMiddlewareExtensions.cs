using System;

using Bytewizer.TinyCLR.Pipeline.Builder;

namespace Bytewizer.Playground.Sockets
{
    public static class HttpResponseExtensions
    {
        public static void UseIpFiltering(this IApplicationBuilder builder, string cidr)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Use(new IpFilteringMiddleware(cidr));
        }
    }
}
