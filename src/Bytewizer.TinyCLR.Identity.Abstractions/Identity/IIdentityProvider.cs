namespace Bytewizer.TinyCLR.Identity
{
    /// <summary>
    /// Represents the default implementation of the <see cref="IIdentityProvider"/> interface.
    /// </summary>
    public interface IIdentityProvider
    {
        IIdentityUser FindByName(string userName);     
        IdentityResult VerifyPassword(IIdentityUser user, byte[] password);
    }
}