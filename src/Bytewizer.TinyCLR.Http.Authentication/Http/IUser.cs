namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Used to provide a minimal interface for a user name and hash.
    /// </summary>
    public interface IUser
    {
        /// <summary>
        /// Gets or sets the unique name for the user.
        /// </summary>
        string Username { get; set; }

        /// <summary>
        /// Gets or sets HA1 user hash.
        /// </summary>
        string HA1 { get; set; }
    }
}