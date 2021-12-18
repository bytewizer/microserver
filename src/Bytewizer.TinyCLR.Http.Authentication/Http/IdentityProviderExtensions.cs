using System;

using Bytewizer.TinyCLR.Identity;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Convenience methods for <see cref="IdentityProvider"/>.
    /// </summary>
    public static class IdentityProviderExtensions
    {
        public static IdentityResult Create(this IdentityProvider provider, IIdentityUser user, string realm, string secret)
        {
            var password = AuthHelper.ComputeA1Hash(user.UserName, realm, secret);

            user.Metadata = password;

            return provider.Create(user, password);
        }
    }
}
