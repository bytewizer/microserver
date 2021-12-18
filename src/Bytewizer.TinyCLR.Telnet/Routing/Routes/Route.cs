namespace Bytewizer.TinyCLR.Telnet
{
    /// <summary>
    /// Implementation of <see cref="Route"/> class.
    /// </summary>
    public class Route
    {
#pragma warning disable IDE1006 // Naming Styles

        /// <summary>
        /// Get or sets the route controller.
        /// </summary>
        public string controller { get; set; }


        /// <summary>
        /// Get or sets the route action.
        /// </summary>
        public string action { get; set; }

#pragma warning restore IDE1006 // Naming Styles
    }
}
