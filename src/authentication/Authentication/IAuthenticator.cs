namespace Bytewizer.TinyCLR.Http.Authentication
{
    public interface IAuthenticator
    {
        string AuthenticationScheme { get; }

        void CreateChallenge(HttpContext context);

        string Authenticate(HttpContext context);
    }
}