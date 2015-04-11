using System;

namespace MicroServer.Net.Http.Routing
{
    /// <summary>
    /// Implementation of <see cref="MappedRoute"/>
    /// </summary>
    public class MappedRoute
    {
        /// <summary>
        /// Gets mapped route name.
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Gets regex pattern.
        /// </summary>
        public string regex { get; set; }

        /// <summary>
        /// Gets the rewrite default path.
        /// </summary>
        public DefaultRoute defaults { get; set; }
    }
}
