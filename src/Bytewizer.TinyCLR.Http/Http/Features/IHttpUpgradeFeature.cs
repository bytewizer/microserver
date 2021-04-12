using System.IO;

namespace Bytewizer.TinyCLR.Http.Features
{
    /// <summary>
    /// A feature interface for websockets server upgrade feature. Use <see cref="HttpContext.Features"/>
    /// to access an instance associated with the current request.
    /// </summary>
    public interface IHttpUpgradeFeature
    {
        /// <summary>
        /// Indicates if the server can upgrade this request to an opaque, bidirectional stream.
        /// </summary>
        bool IsUpgradableRequest { get; }

        /// <summary>
        /// Attempt to upgrade the request to an opaque, bidirectional stream. The response status code
        /// and headers need to be set before this is invoked. Check <see cref="IsUpgradableRequest"/>
        /// before invoking.
        /// </summary>
        Stream Upgrade();
    }
}