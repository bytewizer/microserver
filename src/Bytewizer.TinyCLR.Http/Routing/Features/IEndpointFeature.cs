namespace Bytewizer.TinyCLR.Http.Features
{
    /// <summary>
    /// A feature interface for endpoint routing functionality.
    /// </summary>
    public interface IEndpointFeature
    {
        /// <summary>
        /// Gets or sets the selected <see cref="Endpoint"/> for the current request.
        /// </summary>
        Endpoint Endpoint { get; set; }
    }
}