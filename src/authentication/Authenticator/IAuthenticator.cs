namespace Bytewizer.TinyCLR.Http.Authenticator
{
    public interface IAuthenticator
    {
        void CreateChallenge(HttpContext context, AuthenticationOptions options);

        string Authenticate(HttpContext context, AuthenticationOptions options);
    }
}