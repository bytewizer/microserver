namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Options for the <see cref="DeveloperExceptionPageMiddleware"/>.
    /// </summary>
    public class DeveloperExceptionPageOptions
    {
        /// <summary>
        /// Create an instance with the default options settings.
        /// </summary>
        public DeveloperExceptionPageOptions()
        {
            DisplayStackTrace = true;
        }

        public bool DisplayStackTrace { get; set; }
    }
}
