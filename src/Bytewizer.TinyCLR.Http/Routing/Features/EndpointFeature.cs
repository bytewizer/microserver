namespace Bytewizer.TinyCLR.Http.Features
{
    /// <summary>
    /// A feature for endpoint routing functionality.
    /// </summary>
    public class EndpointFeature : IEndpointFeature
    {
        /// <inheritdoc />
        public Endpoint Endpoint { get; set; }
    }
}
