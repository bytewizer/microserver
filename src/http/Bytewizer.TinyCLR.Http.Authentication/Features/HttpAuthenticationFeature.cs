using Bytewizer.TinyCLR.Identity;

namespace Bytewizer.TinyCLR.Http.Features
{
    ///<inheritdoc/>
    public class HttpAuthenticationFeature : IHttpAuthenticationFeature
    {
        ///<inheritdoc/>
        public IIdentityUser User { get; set; }
    }
}