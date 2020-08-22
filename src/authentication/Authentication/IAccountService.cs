namespace Bytewizer.TinyCLR.Http.Authentication
{
    public interface IAccountService
    {
        IAuthenticationUser Lookup(string userName, string host);
    }
}