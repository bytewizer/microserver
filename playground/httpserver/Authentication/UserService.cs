using System.Collections;

using Bytewizer.TinyCLR.Http.Authenticator;

namespace Bytewizer.TinyCLR.WebServer.Authentication
{
    public class AccountService : IAccountService
    {  
        private readonly Hashtable _users = new Hashtable();

        public AccountService()
        {
            // users hardcoded for simplicity, store in a db with hashed passwords in production applications
            var user = new User { Id = 1, FirstName = "Test", LastName = "User", Username = "test", Password = "test" };
            _users.Add(user.Username, user);
        }

        public IUser Authenticate(string username, string password)
        {
            var user = (User)_users[username];

            if (user.Username == username && user.Password == password)
            {
                // authentication successful so return user details.
                return user;
            }

            // return null if user not found
            return null;
        }
    }
}
