using Bytewizer.TinyCLR.Http.Authenticator;

namespace Bytewizer.TinyCLR.Http
{
    public class AuthenticationOptions
    {
        public AuthenticationOptions()
        {
            AuthenticationScheme = AuthenticationSchemes.None;
            Realm = "tinyclr";
        }

        public AuthenticationSchemes AuthenticationScheme { get; set; }

        public IAccountService AccountService { get; set; }

        public string Realm { get; set; }
    }
}