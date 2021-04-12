using System.IO;

namespace Bytewizer.TinyCLR.Http.Features
{
    ///<inheritdoc/>
    internal class HttpUpgradeFeature : IHttpUpgradeFeature
    {
        ///<inheritdoc/>
        public bool IsUpgradableRequest { get; }

        ///<inheritdoc/>
        public Stream Upgrade()
        {
            return null;
        }
    }
}