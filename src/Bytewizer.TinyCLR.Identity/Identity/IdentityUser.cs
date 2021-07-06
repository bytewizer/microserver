using System;

namespace Bytewizer.TinyCLR.Identity
{
    /// <summary>
    /// Represents the default implementation of the <see cref="IdentityUser"/> class.
    /// </summary>
    public class IdentityUser : IIdentityUser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityUser"/> class.
        /// </summary>
        /// <remarks>The Id property is initialized from a new GUID string value.</remarks>
        /// <param username="user">The user name.</param>
        public IdentityUser(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException(nameof(userName));
            }

            Id = DateTime.Now.Ticks.ToString();
            Name = userName;
        }

        /// <summary>
        /// Gets or sets the primary key for this user.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Gets or sets the user name for this user.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets a hashed representation of the password for this user.
        /// </summary>
        public byte[] PasswordHash { get; set; }

        /// <summary>
        /// Gets or sets a salt value for this user.
        /// </summary>
        public byte[] PasswordSalt { get; set; }
    }
}