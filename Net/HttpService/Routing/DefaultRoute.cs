using System;

namespace MicroServer.Net.Http.Routing
{
    /// <summary>
    /// Implementation of <see cref="DefaultRoute"/> class.
    /// </summary>
    public class DefaultRoute
    {
        /// <summary>
        /// Gets the default controller.
        /// </summary>
        public string controller { get; set; }

        /// <summary>
        /// Gets the default action.
        /// </summary>
        public string action { get; set; }

        /// <summary>
        /// Gets the default action.
        /// </summary>
        public string id { get; set; }
    }
}
