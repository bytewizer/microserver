using Bytewizer.TinyCLR.Identity;

namespace Bytewizer.TinyCLR.Ftp.Features
{
    ///<inheritdoc/>
    public class FtpAuthenticationFeature : IFtpAuthenticationFeature
    {
        ///<inheritdoc/>
        public bool AllowAnonymous { get; set; } = false;

        ///<inheritdoc/>
        public IIdentityProvider IdentityProvider { get; set; }
    }
}