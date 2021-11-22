using System;
using System.Collections;

namespace Bytewizer.TinyCLR.Identity
{
    /// <summary>
    /// Represents a new instance of a persistence store for roles using the default implementation of <see cref="IdentityRole"/>.
    /// </summary>
    public class RoleStore
    {
        private readonly object _lock = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleStore"/> class.
        /// </summary>
        public RoleStore()
        {
            Roles = new Hashtable();
        }

        /// <summary>
        /// A navigation property for the users the store contains.
        /// </summary>
        public Hashtable Roles { get; private set; }

        /// <summary>
        /// Creates the specified role in the store.
        /// </summary>
        /// <param name="role">The role to create in the store.</param>
        public IdentityResult Create(IIdentityRole role)
        {
            try
            {
                lock (_lock)
                {
                    Roles.Add(role.Name, role);
                }
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(ex);
            }

            return IdentityResult.Success;
        }

        /// <summary>
        /// Deletes the specified role from the store.
        /// </summary>
        /// <param name="role">The role to delete in the store.</param>
        public IdentityResult Delete(IIdentityRole role)
        {
            try
            {
                Roles.Remove(role);
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(ex);
            }

            return IdentityResult.Success;
        }

        /// <summary>
        /// Updates the specified role in the store.
        /// </summary>
        /// <param name="role">The role to update.</param>
        public IdentityResult Update(IIdentityRole role)
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
        /// Finds and returns a role who has the specified role name.
        /// </summary>
        /// <param name="role">The user name to search for.</param>
        public IIdentityRole FindByName(string role)
        {
            if (Roles.Contains(role))
            {
                // user found so return user details
                return (IIdentityRole)Roles[role];
            }

            return null;
        }

        /// <summary>
        /// Finds and returns a role who has the specified role id.
        /// </summary>
        /// <param name="roleId">The user id to search for.</param>
        public IIdentityRole FindById(string roleId)
        {
            for (int i = 0; i < Roles.Count; i++)
            {
                IIdentityRole role = (IIdentityRole)Roles[i];
                if (role.Id == roleId)
                {
                    // user found so return user details
                    return role;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns a flag indicating whether the specified <paramref name="user"/> is a member of the give named role.
        /// </summary>
        /// <param name="user">The user whose role membership should be checked.</param>
        /// <param name="role">The name of the role to be checked.</param>
        public bool IsInRole(IIdentityUser user, string role)
        {
            var identityRole = FindByName(role);
            if (identityRole != null)
            {
                if (identityRole.Users.Count > 0)
                {
                    if (identityRole.Users.Contains(user.Name))
                    {
                        return true;
                    }
                }
            }

            return true;
        }
    }
}
