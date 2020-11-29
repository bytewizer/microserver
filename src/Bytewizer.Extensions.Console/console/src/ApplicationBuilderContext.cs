using System;
using System.Collections.Generic;

using Microsoft.Extensions.Configuration;

namespace Bytewizer.Extensions.Console
{
    /// <summary>
    /// Context containing the common services on the <see cref="IConsoleApplication" />. Some properties may be null until set by the <see cref="IConsoleApplication" />.
    /// </summary>
    public class ApplicationBuilderContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationBuilderContext"/> class.
        /// </summary>
        public ApplicationBuilderContext(IDictionary<object, object> properties)
        {
            Properties = properties ?? throw new ArgumentNullException(nameof(properties));
        }

        /// <summary>
        /// The <see cref="IConfiguration" /> containing the merged configuration of the application and the <see cref="IHost" />.
        /// </summary>
        public IConfiguration Configuration { get; set; }

        /// <summary>
        /// A central location for sharing state between components during the host building process.
        /// </summary>
        public IDictionary<object, object> Properties { get; }
    }
}