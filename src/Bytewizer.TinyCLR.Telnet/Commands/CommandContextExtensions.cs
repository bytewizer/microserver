using System;
using System.Collections;
using Bytewizer.TinyCLR.Identity;
using Bytewizer.TinyCLR.Telnet.Features;

namespace Bytewizer.TinyCLR.Telnet
{
    /// <summary>
    /// Extension methods for <see cref="CommandContext"/> related to routing.
    /// </summary>
    public static class CommandContextExtensions
    {
        /// <summary>
        /// Extension method for getting the commands for the current request.
        /// </summary>
        /// <param name="context">The <see cref="CommandContext"/> context.</param>
        public static Hashtable GetCommands(this CommandContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var endpointFeature = (EndpointFeature)context.TelnetContext.Features.Get(typeof(EndpointFeature));

            return endpointFeature?.Commands;
        }

        /// <summary>
        /// Extension method for getting the commands for the current request.
        /// </summary>
        /// <param name="context">The <see cref="CommandContext"/> context.</param>
        public static IIdentityUser GetUserIdentiy(this CommandContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var sessionFeature = (SessionFeature)context.TelnetContext.Features.Get(typeof(SessionFeature));
            var identiyProvider = sessionFeature.IdentityProvider;
            if (identiyProvider == null)
            {
                return null;
            }

            if (identiyProvider.TryGetUser(sessionFeature.UserName, out IIdentityUser user))
            {
                return user;
            }
            else
            {
                return null;
            }
        }
    }
}