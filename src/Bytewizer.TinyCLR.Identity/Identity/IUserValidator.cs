﻿namespace Bytewizer.TinyCLR.Identity
{
    /// <summary>
    /// Provides an abstraction for user validation.
    /// </summary>
    public interface IUserValidator
    {
        /// <summary>
        /// Validates the specified <paramref name="user"/>.
        /// </summary>
        /// <param name="manager">The <see cref="UserManager"/> that can be used to retrieve user properties.</param>
        /// <param name="user">The user to validate.</param>
        IdentityResult Validate(UserManager manager, IIdentityUser user);
    }
}
