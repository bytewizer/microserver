namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Used to provide user accounts from account services.
    /// </summary>
    public interface IAccountProvider
    {
        /// <summary>
        /// Retrive a user from the account service.
        /// </summary>
        /// <param name="username">The user name to retrive</param>
        /// <param name="user">The <see cref="IUser"/> retrived if located</param>
        bool TryGetUser(string username, out IUser user);
    }
}