namespace Bytewizer.TinyCLR.Http.Features
{
    ///<inheritdoc/>
    public class HttpAuthenticationFeature : IHttpAuthenticationFeature
    {
        ///<inheritdoc/>
        public IUser User { get; set; }
    }
}