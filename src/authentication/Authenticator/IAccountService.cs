namespace Bytewizer.TinyCLR.Http.Authenticator
{
    public interface IAccountService
    {
        IAuthenticationUser Lookup(string userName, string host);
    }
}