using System;

using Bytewizer.TinyCLR.Pipeline.Builder;

namespace Bytewizer.TinyCLR.Sockets
{
    public static class IpFilteringExtensions
    {
        public static IApplicationBuilder UseIpFiltering(this IApplicationBuilder builder, string cidr)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder.Use(new IpFilteringMiddleware(cidr));
        }
    }
}
