namespace Bytewizer.TinyCLR.Http.Authenticator
{
    public interface IUser
    {
        /// <summary>
        /// The unique name for the user.
        /// </summary>
        string Username { get; set; }
    }
}