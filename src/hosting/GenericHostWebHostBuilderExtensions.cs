using System;
using System.Collections;
using System.Text;
using System.Threading;

namespace Bytewizer.TinyCLR.Hosting
{
    public static class GenericHostWebHostBuilderExtensions
    {
        public static IHostBuilder ConfigureWebHost(this IHostBuilder builder, WebHostBuilderDelegate configure)
        {
            var webhostBuilder = new GenericWebHostBuilder(builder);
            configure(webhostBuilder);
            //builder.ConfigureServices((context, services) => services.AddHostedService<GenericWebHostService>());
            return builder;
        }
    }
}
