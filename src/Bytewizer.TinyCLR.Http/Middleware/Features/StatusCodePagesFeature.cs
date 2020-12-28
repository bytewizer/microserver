
namespace Bytewizer.TinyCLR.Http.Features
{
    /// <summary>
    /// Represents the Status code pages feature.
    /// </summary>
    public class StatusCodePagesFeature : IStatusCodePagesFeature
    {
        /// <summary>
        /// Enables or disables status code pages. The default value is true.
        /// Set this to false to prevent the <see cref="StatusCodePagesMiddleware"/>
        /// from creating a response body while handling the error status code.
        /// </summary>
        public bool Enabled { get; set; } = true;
    }
}
