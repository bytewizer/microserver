using System;

using Bytewizer.TinyCLR.Http.Authenticator;

namespace Bytewizer.TinyCLR.Http
{
    public class AuthenticationOptions
    {

        public AuthenticationOptions(IAccountService userService)
        {
            if (userService == null)
                throw new ArgumentNullException(nameof(userService));

                Authenticator = new Authentication(userService);
            UserService = userService;
        }

        public IAuthenticator Authenticator { get; } 

        public IAccountService UserService { get; }

        public string Realm { get; set; }
    }
}