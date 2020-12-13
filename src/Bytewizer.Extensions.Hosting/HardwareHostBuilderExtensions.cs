using Bytewizer.TinyCLR.Hardware;
using Bytewizer.TinyCLR.DependencyInjection;

namespace Bytewizer.TinyCLR.Hosting
{
    /// <summary>
    /// Contains extensions for an <see cref="IHostBuilder"/>.
    /// </summary>
    public static class HardwareHostBuilderExtensions
    {
        /// <summary>
        /// Adds and configures hardware support.
        /// </summary>
        public static IHostBuilder ConfigureHardware(this IHostBuilder builder, HardwareOptionsDelegate options)
        {
            var hardware = new Mainboard(options).Connect();

            builder.ConfigureServices((context, services) => services.AddSingleton(typeof(IHardware), hardware));

            return builder;
        }
    }
}
