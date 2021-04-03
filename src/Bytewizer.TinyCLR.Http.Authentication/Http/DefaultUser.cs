namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// The default user added to <see cref="IAccountProvider"/>.
    /// </summary>
    public class DefaultUser : IUser
    {
        /// <summary>
        /// The user name allow for login.
        /// </summary>
        public string Username { get; set; }
        
        /// <summary>
        /// The MD5 HA1 hash.
        /// </summary>
        public string HA1 { get; set; }
    }
}
