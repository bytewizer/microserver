using System;

namespace Bytewizer.TinyCLR.Hosting
{
    /// <summary>
    /// Contains extension methods for configuring the <see cref="IWebHostBuilder" />.
    /// </summary>
    public static class WebHostBuilderExtensions
    {
        /// <summary>
        /// Specify the assembly containing the startup type to be used by the web host.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="IWebHostBuilder"/> to configure.</param>
        /// <returns>The <see cref="IWebHostBuilder"/>.</returns>
        public static IWebHostBuilder UseStartup(this IWebHostBuilder hostBuilder, Type type)
        {
            var startup = Activator.CreateInstance(type);
            if (startup == null)
            {
                throw new InvalidOperationException(nameof(type));
            }



            return hostBuilder;
        }

        /// <summary>
        /// Specify the content root directory to be used by the web host.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="IWebHostBuilder"/> to configure.</param>
        /// <param name="contentRoot">Path to root directory of the application.</param>
        /// <returns>The <see cref="IWebHostBuilder"/>.</returns>
        public static IWebHostBuilder UseContentRoot(this IWebHostBuilder hostBuilder, string contentRoot)
        {
            if (contentRoot == null)
            {
                throw new ArgumentNullException(nameof(contentRoot));
            }

            //return hostBuilder.UseSetting(WebHostDefaults.ContentRootKey, contentRoot);
            return hostBuilder;
        }
    }
}