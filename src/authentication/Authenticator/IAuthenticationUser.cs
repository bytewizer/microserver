namespace Bytewizer.TinyCLR.Http.Authenticator
{
    public interface IAuthenticationUser
    {
        string Username { get; set; }

        string Password { get; set; }

        string HA1 { get; set; }
    }
}