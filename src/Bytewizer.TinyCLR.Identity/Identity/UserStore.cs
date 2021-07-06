using System;
using System.Collections;

namespace Bytewizer.TinyCLR.Identity
{
    /// <summary>
    /// Represents a new instance of a persistence store for users using the default implementation of <see cref="IdentityUser"/>.
    /// </summary>
    public class UserStore
    {
        private readonly object _lock = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="UserStore"/> class.
        /// </summary>
        public UserStore()
        {
            Users = new Hashtable();
        }

        /// <summary>
        /// A navigation property for the users the store contains.
        /// </summary>
        public Hashtable Users { get; private set; }

        /// <summary>
        /// Creates the specified user in the store.
        /// </summary>
        /// <param name="user">The user to create.</param>
        public IdentityResult Create(IIdentityUser user)
        {
            try
            {
                lock (_lock)
                {
                    Users.Add(user.Name, user);
                }
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(ex);
            }

            return IdentityResult.Success;
        }

        /// <summary>
        /// Deletes the specified user from the store.
        /// </summary>
        /// <param name="user">The user to delete.</param>
        public IdentityResult Delete(IIdentityUser user)
        {
            try
            {
                Users.Remove(user);
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(ex);
            }

            return IdentityResult.Success;
        }

        /// <summary>
        /// Updates the specified user in the store.
        /// </summary>
        /// <param name="user">The user to update.</param>
        public IdentityResult Update(IIdentityUser user)
        {
            try
            {
                // TODO: Create update method
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(ex);
            }

            return IdentityResult.Success;
        }

        /// <summary>
        /// Determines whether the user has a password set.
        /// </summary>
        /// <param name="user"></param>
        public virtual bool HasPassword(IIdentityUser user)
        {
            if (TryGetUser(user.Name, out user))
            {
                if (user.PasswordHash == null)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Removes all the users in the store.
        /// </summary>
        public void Clear()
        {
            Users.Clear();
        }

        /// <summary>
        /// Retrive the specified user from the user store.
        /// </summary>
        /// <param name="username">The user name to retrive.</param>
        /// <param name="user">The <see cref="IIdentityUser"/> retrived if located.</param>
        public bool TryGetUser(string username, out IIdentityUser user)
        {
            user = null;

            if (Users.Contains(username))
            {
                // user found so return user details
                user = (IIdentityUser)Users[username];
                return true;
            }

            // return false if user not found
            return false;
        }

        /// <summary>
        /// Finds and returns a user who has the specified user name.
        /// </summary>
        /// <param name="username">The user name to search for.</param>
        public IIdentityUser FindByName(string username)
        {
            if (Users.Contains(username))
            {
                // user found so return user details
                return (IIdentityUser)Users[username];
            }

            return null;
        }

        /// <summary>
        /// Finds and returns a user who has the specified user id.
        /// </summary>
        /// <param name="userId">The user id to search for.</param>
        public IIdentityUser FindById(string userId)
        {
            for (int i = 0; i < Users.Count; i++)
            {
                IIdentityUser user = (IIdentityUser)Users[i];
                if (user.Id == userId)
                {
                    // user found so return user details
                    return user;
                }
            }

            return null;
        }

        /// <summary>
        /// Get the password hash for a user
        /// </summary>
        /// <param name="user"></param>
        public byte[] GetPasswordHash(IIdentityUser user)
        {
            IIdentityUser identityUser = FindByName(user.Name);
            if (identityUser != null)
            {
                return identityUser.PasswordHash;
            }

            return null;
        }
    }
}
