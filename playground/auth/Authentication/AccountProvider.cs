using System.Collections;

using Bytewizer.TinyCLR.Http;

namespace Bytewizer.Playground.Authentication
{
    public class AccountProvider : IAccountProvider
    {
        private readonly Hashtable _users = new Hashtable();

        public AccountProvider()
        {
            // users hardcoded for simplicity, store in a db with hashed passwords in production applications
            _users.Add("bsmith", new User
            {
                Id = 1,
                FirstName = "Bob",
                LastName = "Smith",
                Username = "bsmith",
                HA1 = AuthHelper.ComputeA1Hash("bsmith", "password")
            });
            _users.Add("ksmith", new User
            {
                Id = 2,
                FirstName = "Kim",
                LastName = "Smith",
                Username = "ksmith",
                HA1 = AuthHelper.ComputeA1Hash("ksmith", "password")
            });
        }

        public bool TryGetUser(string username, out IUser user)
        {
            user = null;
            if (_users.Contains(username))
            {
                user = (IUser)_users[username];
                return true;
            }

            return false;
        }
    }
}
