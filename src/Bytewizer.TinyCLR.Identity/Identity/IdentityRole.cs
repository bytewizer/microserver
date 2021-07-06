using System;
using System.Collections;

namespace Bytewizer.TinyCLR.Identity
{
    /// <summary>
    /// Represents the default implementation of the <see cref="IdentityRole"/> class.
    /// </summary>
    public class IdentityRole : IIdentityRole
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityRole"/> class.
        /// </summary>
        /// <remarks>The Id property is initialized to from a new GUID string value.</remarks>
        public IdentityRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentNullException(nameof(roleName));
            }

            Id = DateTime.Now.Ticks.ToString();
            Name = roleName;
            Users = new Hashtable();
        }

        /// <inheritdoc/>
        public string Id { get; private set; }

        /// <inheritdoc/>
        public string Name { get; set; }
        
        /// <inheritdoc/>
        public Hashtable Users { get; private set; }
    }
}
