using System.Collections;

using Bytewizer.TinyCLR.Http;

namespace Bytewizer.Playground.Authentication
{
    public class AccountService : IAccountService
    {
        private readonly Hashtable _users = new Hashtable();

        public AccountService()
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

        public IUser GetUser(string username)
        {
            if (_users.Contains(username))
            {
                // authentication successful so return user details
                return (IUser)_users[username];
            }

            // return null if user not found
            return null;
        }
    }
}
