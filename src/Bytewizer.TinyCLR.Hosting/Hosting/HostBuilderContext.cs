using System;
using System.Collections;

namespace Bytewizer.TinyCLR.Hosting
{
    /// <summary>
    /// Context containing the common services on the <see cref="IHost" />. Some properties may be null until set by the <see cref="IHost" />.
    /// </summary>
    public class HostBuilderContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HostBuilderContext"/> class.
        /// </summary>
        public HostBuilderContext(IDictionary properties)
        {
            Properties = properties ?? throw new ArgumentNullException(nameof(properties));
        }

        /// <summary>
        /// A central location for sharing state between components during the host building process.
        /// </summary>
        public IDictionary Properties { get; }
    }
}