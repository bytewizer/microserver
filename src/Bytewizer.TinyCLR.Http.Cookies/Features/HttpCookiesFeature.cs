namespace Bytewizer.TinyCLR.Http.Features
{
    ///<inheritdoc/>
    public class HttpCookiesFeature : IHttpCookiesFeature
    {
        ///<inheritdoc/>
        public ICookieCollection Cookies { get; set; }

        ///<inheritdoc/>
        public IResponseCookies ResponseCookies { get; set; }
    }
}