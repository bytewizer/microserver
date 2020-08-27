using System;

namespace Bytewizer.TinyCLR.Hosting
{
    public static class GenericHostBuilderExtensions
    {
        public static IHostBuilder ConfigureWebHostDefaults(this IHostBuilder builder, WebHostBuilderDelegate configure)
        {
            if (configure is null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            return builder.ConfigureWebHost(webHostBuilder =>
            {
                WebHost.ConfigureWebDefaults(webHostBuilder);

                configure(webHostBuilder);
            });
        }
    }
}
