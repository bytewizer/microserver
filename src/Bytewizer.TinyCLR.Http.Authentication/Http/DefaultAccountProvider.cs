using System.Collections;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Represents the default implementation of the <see cref="IAccountProvider"/> interface.
    /// </summary>
    public class DefaultAccountProvider : IAccountProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultAccountProvider"/> class.
        /// </summary>
        public DefaultAccountProvider()
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
        public bool TryGetUser(string username, out IUser user)
        {
            user = null;

            if (Users.Contains(username))
            {
                // authentication successful so return user details
                user = (IUser)Users[username];
                return true;
            }

            // return false if user not found
            return false;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="secret"></param>
        public void CreateUser(string username, string secret)
        {
            CreateUser(username, AuthHelper.DefaultRealm, secret);
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="realm"></param>
        /// <param name="secret"></param>
        public void CreateUser(string username, string realm, string secret)
        {
            var user = new DefaultUser()
            {
                Username = username,
                HA1 = AuthHelper.ComputeA1Hash(username, realm, secret)
            };
            
            Users.Add(username, user);
        }

        /// <summary>
        /// Deletes a user from the provider.
        /// </summary>
        /// <param name="username">The name of the user to delete.</param>
        public bool DeleteUser(string username)
        {
            if (Users.Contains(username))
            {
                Users.Remove(username);
                return true;
            }

            return false;
        }
    }
}