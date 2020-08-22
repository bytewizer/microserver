using Bytewizer.TinyCLR.Http.Authentication;

namespace Bytewizer.TinyCLR.Http
{
    public class AuthenticationOptions
    {
        public AuthenticationOptions(IAuthenticator authenticator)
        {
            Authenticator = authenticator;
        }

        public AuthenticationOptions(IAccountService userService, string realm)
        {
            Authenticator = new BasicAuthentication(userService, realm);
            UserService = userService;
            Realm = realm;
        }

        public IAuthenticator Authenticator { get; } 

        public IAccountService UserService { get; }

        public string Realm { get; }

        //public class Builder
        //{
        //    public Builder()
        //    {
        //    }

        //    public AuthenticationOptions Build()
        //    {
        //        return new AuthenticationOptions();
        //    }
        //}
    }
}