using System;
using System.Collections;
using Bytewizer.TinyCLR.Identity;
using Bytewizer.TinyCLR.Terminal.Features;

namespace Bytewizer.TinyCLR.Terminal
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
        public static string GetUserName(this CommandContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var endpointFeature = (SessionFeature)context.TerminalContext.Features.Get(typeof(SessionFeature));

            return endpointFeature?.UserName;
        }

        /// <summary>
        /// Extension method for getting the commands for the current request.
        /// </summary>
        /// <param name="context">The <see cref="CommandContext"/> context.</param>
        public static Hashtable GetAvailableCommands(this CommandContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var endpointFeature = (EndpointFeature)context.TerminalContext.Features.Get(typeof(EndpointFeature));
            return endpointFeature?.AvailableCommands;
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

            var sessionFeature = (SessionFeature)context.TerminalContext.Features.Get(typeof(SessionFeature));
            var identiyProvider = sessionFeature?.IdentityProvider;
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