using System;

using Bytewizer.TinyCLR.Http.Authentication;

namespace Bytewizer.TinyCLR.WebServer
{
    public class UserService : IAccountService
    {
        public IAuthenticationUser Lookup(string userName, string host)
        {
            // perform user lookup and return an IAuthenticationUser or null
            return new AdminUser();
        }
    }

    public class AdminUser : IAuthenticationUser
    {
        public string Username { get => "username" ; set => throw new NotImplementedException(); }
        public string Password { get => "password"; set => throw new NotImplementedException(); }
        public string HA1 { get => string.Empty; set => throw new NotImplementedException(); }
    }
}