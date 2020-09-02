namespace Bytewizer.TinyCLR.Http.Authenticator
{
    public interface IAccountService
    {
        IUser Authenticate(string username, string password);
    }
}