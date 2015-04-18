namespace MicroServer.Net.Http.Modules
{
    /// <summary>
    /// Used to control module behavior
    /// </summary>
    public enum ModuleResult
    {
        /// <summary>
        /// Continue with the next module
        /// </summary>
        Continue,

        /// <summary>
        /// Stop processing more modules
        /// </summary>
        Stop,
    }
}