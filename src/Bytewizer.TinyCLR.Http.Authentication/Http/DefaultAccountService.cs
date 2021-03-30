using System.Collections;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Represents the default implementation of the <see cref="IAccountService"/> interface.
    /// </summary>
    public class DefaultAccountService : IAccountService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultAccountService"/> class.
        /// </summary>
        public DefaultAccountService()
        {
            Users = new Hashtable();
        }

        /// <summary>
        /// <summary>
        /// The cross reference table of users and HA1 hashes.
        /// </summary>
        /// </summary>
        public Hashtable Users { get; private set; }


        /// <inheritdoc/>
        public IUser GetUser(string username)
        {
            if (Users.Contains(username))
            {
                // authentication successful so return user details
                return (IUser)Users[username];
            }

            // return null if user not found
            return null;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="secret"></param>
        public void Register(string username, string secret)
        {
            Register(username, AuthHelper.DefaultRealm, secret);
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="realm"></param>
        /// <param name="secret"></param>
        public void Register(string username, string realm, string secret)
        {
            var user = new DefaultUser()
            {
                Username = username,
                HA1 = AuthHelper.ComputeA1Hash(username, realm, secret)
            };
            
            Users.Add(username, user);
        }
    }
}