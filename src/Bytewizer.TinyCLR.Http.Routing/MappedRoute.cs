namespace Bytewizer.TinyCLR.Http.Routing
{
    /// <summary>
    /// Implementation of <see cref="MappedRoute"/>
    /// </summary>
    public class MappedRoute
    {
        /// <summary>
        /// Gets and sets the route name.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Gets and sets the route pattern.
        /// </summary>
        public string Pattern { get; set; }

        /// <summary>
        /// Gets and sets the regex route pattern.
        /// </summary>
        public string Regex { get; set; }

        /// <summary>
        /// Gets and sets the controller route path.
        /// </summary>
        public Route Defaults { get; set; }
    }
}
