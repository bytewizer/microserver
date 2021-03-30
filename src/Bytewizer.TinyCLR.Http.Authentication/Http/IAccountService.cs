namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Used to provide user accounts from account services.
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Retrive a user from the account service.
        /// </summary>
        /// <param name="username">The user name to retrive</param>

        IUser GetUser(string username);
    }
}