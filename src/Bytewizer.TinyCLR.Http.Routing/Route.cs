namespace Bytewizer.TinyCLR.Http.Routing
{
    /// <summary>
    /// Implementation of <see cref="Route"/> class.
    /// </summary>
    public class Route
    {
        /// <summary>
        /// Get or sets the route controller.
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// Get or sets the route action.
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Get or sets the route id.
        /// </summary>
        public string Id { get; set; }
    }
}
