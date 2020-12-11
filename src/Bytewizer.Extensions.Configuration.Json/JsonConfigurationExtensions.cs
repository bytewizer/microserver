using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Threading;

namespace Bytewizer.Extensions.Configuration.JSON
{
    /// <summary>
    /// Extension methods for adding <see cref="JsonConfigurationProvider"/>.
    /// </summary>
    public static class JsonConfigurationExtensions
    {
        /// <summary>
        /// Adds a JSON configuration source to <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="stream">The <see cref="Stream"/> to read the json configuration data from.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        //public static IConfigurationBuilder AddJsonStream(this IConfigurationBuilder builder, Stream stream)
        //{
        //    if (builder == null)
        //    {
        //        throw new ArgumentNullException(nameof(builder));
        //    }

        //    return builder.Add(s => s.Stream = stream);
        //}
    }
}
