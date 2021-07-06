using System.Collections;

namespace Bytewizer.TinyCLR.Identity
{
    /// <summary>
    /// Represents the default implementation of the <see cref="IdentityRole"/> interface.
    /// </summary>
    public interface IIdentityRole
    {
        /// <summary>
        /// Gets or sets the primary key for this role.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Gets or sets the name for this role.
        /// </summary>
        string Name { get; set; }
        
        /// <summary>
        /// Gets or sets the users in this role.
        /// </summary>
        Hashtable Users { get; }
    }
}