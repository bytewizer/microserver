namespace Bytewizer.TinyCLR.Identity
{
    /// <summary>
    /// Provides an abstraction for hashing passwords.
    /// </summary>
    public interface IPasswordHasher
    {
        /// <summary>
        /// Returns a hashed representation of the supplied <paramref name="password"/> for the specified <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user whose password is to be hashed.</param>
        /// <param name="password">The password to hash.</param>
        /// <returns>A hashed representation of the supplied <paramref name="password"/> for the specified <paramref name="user"/>.</returns>
        byte[] HashPassword(IIdentityUser user, byte[] password);

        /// <summary>
        /// Returns a <see cref="IdentityResult"/> indicating the result of a password hash comparison.
        /// </summary>
        /// <param name="user">The user whose password should be verified.</param>
        /// <param name="hashedPassword">The hash value for a user's stored password.</param>
        /// <param name="providedPassword">The password supplied for comparison.</param>
        IdentityResult VerifyHashedPassword(IIdentityUser user, byte[] hashedPassword, byte[] providedPassword);
    }
}
