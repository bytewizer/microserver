using System;
using System.Collections;

namespace Bytewizer.TinyCLR.Configuration
{
    /// <summary>
    /// Extensions method for <see cref="IConfigurationRoot"/>
    /// </summary>
    internal static class InternalConfigurationRootExtensions
    {
        /// <summary>
        /// Gets the immediate children sub-sections of configuration root based on key.
        /// </summary>
        /// <param name="root">Configuration from which to retrieve sub-sections.</param>
        /// <param name="path">Key of a section of which children to retrieve.</param>
        /// <returns>Immediate children sub-sections of section specified by key.</returns>
        internal static IEnumerable GetChildrenImplementation(this IConfigurationRoot root, string path)
        {
            return root.Providers;
                //TODO:
                //.Aggregate(Enumerable.Empty<string>(),
                //    (seed, source) => source.GetChildKeys(seed, path))
                //.Distinct(StringComparer.OrdinalIgnoreCase)
                //.Select(key => root.GetSection(path == null ? key : ConfigurationPath.Combine(path, key)));
        }
    }
}
