using System;
using System.Collections;

namespace Bytewizer.TinyCLR.Identity
{
    /// <summary>
    /// Provides the APIs for managing user in a persistence store.
    /// </summary>
    public class IdentityProvider : IIdentityProvider
    {
        private readonly UserStore _userStore;
        private readonly RoleStore _roleStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityProvider"/> class.
        /// </summary>
        public IdentityProvider()
            : this(new PasswordHasher())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityProvider"/> class.
        /// </summary>
        /// <param name="passwordHasher">The password hashing implementation to use when saving passwords.</param>
        public IdentityProvider(IPasswordHasher passwordHasher)
        {
            PasswordHasher = passwordHasher;

            _userStore = new UserStore();
            _roleStore = new RoleStore();
        }

        /// <summary>
        /// The <see cref="IPasswordHasher"/> used to hash passwords.
        /// </summary>
        public IPasswordHasher PasswordHasher { get; private set; }

        /// <summary>
        /// The <see cref="IUserValidator"/> used to validate users.
        /// </summary>
        public ArrayList UserValidators { get; } = new ArrayList(0);

        /// <summary>
        /// The <see cref="IPasswordValidator"/> used to validate passwords.
        /// </summary>
        public ArrayList PasswordValidators { get; } = new ArrayList(0);

        /// <summary>
        /// Gets the persistence store of users the manager operates over.
        /// </summary>
        public Hashtable Users { get => _userStore.Users; }

        /// <summary>
        /// Gets the persistence store of roles the manager operates over.
        /// </summary>
        public Hashtable Roles { get => _roleStore.Roles; }

        /// <summary>
        /// Creates the specified <paramref name="user"/> in the user store with no password.
        /// </summary>
        /// <param name="user">The user to create.</param>
        /// <param name="password">The password for the user to hash and store.</param>
        public virtual IdentityResult Create(IIdentityUser user, byte[] password)
        {
            var errors = new ArrayList();

            var userResults = ValidateUser(user);
            if (!userResults.Succeeded)
            {
                errors.AddRange(userResults.Errors);
            }
            var passwordResults = ValidatePassword(user, password);
            if (!passwordResults.Succeeded)
            {
                errors.AddRange(passwordResults.Errors);
            }     
            var updateResults  = UpdatePasswordHash(user, password, false);
            if (!updateResults.Succeeded)
            {
                errors.AddRange(updateResults.Errors);
            }   
            if (errors.Count > 0)
            {
                //Logger.LogWarning(LoggerEventIds.UserValidationFailed, "User validation failed: {errors}.", string.Join(";", errors.Select(e => e.Code)));
                return IdentityResult.Failed(errors);
            }

            return _userStore.Create(user);
        }

        /// <summary>
        /// Deletes the specified <paramref name="user"/> from the user store.
        /// </summary>
        /// <param name="user">The user to delete.</param>
        public virtual IdentityResult Delete(IIdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return _userStore.Delete(user);
        }

        /// <summary>
        /// Updates the specified <paramref name="user"/> in the user store.
        /// </summary>
        /// <param name="user">The user to update.</param>
        public virtual IdentityResult Update(IIdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return _userStore.Update(user);
        }

        /// <summary>
        /// Gets a flag indicating whether the specified <paramref name="user"/> has a password.
        /// </summary>
        /// <param name="user">The user to return a flag for, indicating whether they have a password or not.</param>
        public virtual bool HasPassword(IIdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return _userStore.HasPassword(user);
        }

        /// <summary>
        /// Finds and returns a user, if any, who has the specified user name.
        /// </summary>
        /// <param name="userName">The user name to search for.</param>
        public virtual IIdentityUser FindByName(string userName)
        {
            if (userName == null)
            {
                throw new ArgumentNullException(nameof(userName));
            }

            return _userStore.FindByName(userName);
        }

        /// <summary>
        /// Finds and returns a user, if any, who has the specified <paramref name="userId"/>.
        /// </summary>
        /// <param name="userId">The user id to search for.</param>
        public virtual IIdentityUser FindById(string userId)
        {
            return _userStore.FindById(userId);
        }

        /// <summary>
        /// Add the specified <paramref name="user"/> to the named roles.
        /// </summary>
        /// <param name="user">The user to add to the named roles.</param>
        /// <param name="role">The name of the role to add the user to.</param>
        public virtual IdentityResult AddToRole(IIdentityUser user, IIdentityRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return AddToRoles(user, new IIdentityRole[1] { role });
        }

        /// <summary>
        /// Add the specified <paramref name="user"/> to the named roles.
        /// </summary>
        /// <param name="user">The user to add to the named roles.</param>
        /// <param name="roles">The name of the roles to add the user to.</param>
        public virtual IdentityResult AddToRoles(IIdentityUser user, IIdentityRole[] roles)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (roles == null)
            {
                throw new ArgumentNullException(nameof(roles));
            }

            var errors = new ArrayList();
            foreach (IIdentityRole role in roles)
            {
                var identityRole = _roleStore.FindByName(role.Name);
                if (identityRole == null)
                {
                    var result = _roleStore.Create(role);
                    if (result.Succeeded)
                    {
                        identityRole = _roleStore.FindByName(role.Name);
                        if (identityRole != null)
                        {
                            try
                            {
                                identityRole.Users.Add(user.Name, user);
                            }
                            catch (Exception ex)
                            {
                                return IdentityResult.Failed(ex);
                            }
                        }
                    }
                    else
                    {
                        errors.AddRange(result.Errors);
                    }
                }
                else
                {
                    identityRole.Users.Add(user.Name, user);
                }
            }
            if (errors.Count > 0)
            {
                //Logger.LogWarning(LoggerEventIds.UserValidationFailed, "User validation failed: {errors}.", string.Join(";", errors.Select(e => e.Code)));
                return IdentityResult.Failed(errors);
            }

            return IdentityResult.Success;
        }

        /// <summary>
        /// Removes the specified <paramref name="user"/> from the named role.
        /// </summary>
        /// <param name="user">The user to remove from the named role.</param>
        /// <param name="roles">The name of the roles to remove the user from.</param>
        public virtual IdentityResult RemoveFromRoles(IIdentityUser user, IdentityRole[] roles)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (roles == null)
            {
                throw new ArgumentNullException(nameof(roles));
            }

            var errors = new ArrayList();
            foreach (IdentityRole role in roles)
            {
                var identityRole = _roleStore.FindByName(role.Name);
                if (identityRole != null)
                {
                    try
                    {
                        identityRole.Users.Remove(user.Name);
                    }
                    catch (Exception ex)
                    {
                        return IdentityResult.Failed(ex);
                    }

                    if (identityRole.Users.Count == 0)
                    {
                        var result = _roleStore.Delete(identityRole);
                        if (!result.Succeeded)
                        {
                            errors.AddRange(result.Errors);
                        }
                    }
                }
            }
            if (errors.Count > 0)
            {
                //Logger.LogWarning(LoggerEventIds.UserValidationFailed, "User validation failed: {errors}.", string.Join(";", errors.Select(e => e.Code)));
                return IdentityResult.Failed(errors);
            }

            return IdentityResult.Success;
        }

        /// <summary>
        /// Returns a flag indicating whether the specified <paramref name="user"/> is a member of the given named role.
        /// </summary>
        /// <param name="user">The user whose role membership should be checked.</param>
        /// <param name="role">The name of the role to be checked.</param>
        public virtual bool IsInRole(IIdentityUser user, IIdentityRole role)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            
            return _roleStore.IsInRole(user,role.Name);
        }

        /// <summary>
        /// Gets a list of role names the specified <paramref name="user"/> belongs to.
        /// </summary>
        /// <param name="user">The user whose role names to retrieve.</param>
        public virtual Hashtable GetRoles(IIdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var roles = new Hashtable();

            foreach (DictionaryEntry item in _roleStore.Roles)
            {
                if (IsInRole(user, (IIdentityRole)item.Value))
                {
                    roles.Add(item.Key, item.Value);
                }
            }

            return roles;
        }

        /// <summary>
        /// Returns a list of users from the user store who are members of the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The name of the role whose users should be returned.</param>
        public virtual Hashtable GetUsersInRole(IIdentityRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            var users = new Hashtable();

            foreach (DictionaryEntry item in _userStore.Users)
            {
                if (IsInRole((IIdentityUser)item.Value, role))
                {
                    users.Add(item.Key, item.Value);
                }
            }

            return users;
        }

        /// <summary>
        /// Determines whether the password is valid for the user.
        /// </summary>
        /// <param name="user">The user whose password should be verified.</param>
        /// <param name="password">The password to verify.</param>
        public virtual bool CheckPassword(IIdentityUser user, byte[] password)
        {
                var validate = VerifyPassword(user, password);
                if (validate.Succeeded)
                {
                    return true;
                }

                return false;
        }

        /// <summary>
        /// Returns a <see cref="IdentityResult"/> indicating the result of a password hash comparison.
        /// </summary>
        /// <param name="user">The user whose password should be verified.</param>
        /// <param name="password">The password to verify.</param>
        public virtual IdentityResult VerifyPassword(IIdentityUser user, byte[] password)
        {
            var hash =  _userStore.GetPasswordHash(user);
            if (hash == null)
            {
                return IdentityResult.Failed();
            }

            return PasswordHasher.VerifyHashedPassword(user, hash, password);
        }

        /// <summary>
        /// Updates a user's password hash.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="newPassword">The new password.</param>
        /// <param name="validatePassword">Whether to validate the password.</param>
        /// <returns>Whether the password has was successfully updated.</returns>
        public virtual IdentityResult UpdatePasswordHash(IIdentityUser user, byte[] newPassword, bool validatePassword)
        {
            if (validatePassword)
            {
                var validate = ValidatePassword(user, newPassword);
                if (!validate.Succeeded)
                {
                    return validate;
                }
            }

            user.PasswordHash = newPassword != null ? PasswordHasher.HashPassword(user, newPassword) : null;
            
            return IdentityResult.Success;
        }

        /// <summary>
        /// Should return <see cref="IdentityResult.Success"/> if validation is successful. This is
        /// called before saving the user via Create or Update.
        /// </summary>
        /// <param name="user">The user</param>
        /// <returns>A <see cref="IdentityResult"/> representing whether validation was successful.</returns>
        public virtual IdentityResult ValidateUser(IIdentityUser user)
        {
            var errors = new ArrayList();
            foreach (IUserValidator Validator in UserValidators)
            {
                var result = Validator.Validate(this, user);
                if (!result.Succeeded)
                {
                    errors.AddRange(result.Errors);
                }
            }
            if (errors.Count > 0)
            {
                //Logger.LogWarning(13, "User {userId} validation failed: {errors}.", await GetUserIdAsync(user), string.Join(";", errors.Select(e => e.Code)));
                return IdentityResult.Failed(errors);
            }
            return IdentityResult.Success;
        }

        /// <summary>
        /// Should return <see cref="IdentityResult.Success"/> if validation is successful. This is
        /// called before updating the password hash.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        /// <returns>A <see cref="IdentityResult"/> representing whether validation was successful.</returns>
        public virtual IdentityResult ValidatePassword(IIdentityUser user, byte[] password)
        {
            var errors = new ArrayList();
            foreach (IPasswordValidator Validator in PasswordValidators)
            {
                var result = Validator.Validate(this, user, password);
                if (!result.Succeeded)
                {
                    errors.AddRange(result.Errors);
                }
            }
            if (errors.Count > 0)
            {
                //Logger.LogWarning(14, "User {userId} password validation failed: {errors}.", await GetUserIdAsync(user), string.Join(";", errors.Select(e => e.Code)));
                return IdentityResult.Failed(errors);
            }
            return IdentityResult.Success;
        }
    }
}