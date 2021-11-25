namespace Bytewizer.TinyCLR.Identity
{
    /// <summary>
    /// Represents the default implementation of the <see cref="IIdentityUser"/> interface.
    /// </summary>
    public interface IIdentityUser
    {
        /// <summary>
        /// Gets or sets the primary key for this user.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Gets or sets the user name for this user.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets or sets a hashed representation of the password for this user.
        /// </summary>
        byte[] PasswordHash { get; set; }

        /// <summary>
        /// Gets or sets a salt value for this user.
        /// </summary>
        byte[] PasswordSalt { get; set; }
    }
}