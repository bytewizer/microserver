namespace Bytewizer.TinyCLR.Http.Features
{
    /// <summary>
    /// Represents the Status code pages feature.
    /// </summary>
    public interface IStatusCodePagesFeature
    {
        /// <summary>
        /// Indicates if the status code middleware will handle responses.
        /// </summary>
        bool Enabled { get; set; }
    }
}
