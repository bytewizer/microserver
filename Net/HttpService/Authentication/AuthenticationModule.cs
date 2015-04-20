using System;
using System.Net;

using MicroServer.Net.Http;
using MicroServer.Net.Http.Modules;
using MicroServer.Net.Http.Exceptions;
using Microsoft.SPOT;

namespace MicroServer.Net.Http.Authentication
{
    /// <summary>
    /// Uses <see cref="IAuthenticator"/> to authenticate requests and then <see cref="IPrincipalFactory"/> to generate the user information.
    /// </summary>
    public class AuthenticationModule : IAuthenticationModule
    {
        private readonly IAuthenticator _authenticator;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationModule" /> class.
        /// </summary>
        /// <param name="authenticator">Used for the actual authentication.</param>
        /// <exception cref="System.ArgumentNullException">authenticator</exception>
        public AuthenticationModule(IAuthenticator authenticator)
        {
            if (authenticator == null) throw new ArgumentNullException("authenticator");
            _authenticator = authenticator;
        }

        #region IAuthenticationModule Members

        /// <summary>
        /// Invoked before anything else
        /// </summary>
        /// <param name="context">HTTP context</param>
        /// <remarks>
        /// <para>The first method that is executed in the pipeline.</para>
        /// Try to avoid throwing exceptions if you can. Let all modules have a chance to handle this method. You may break the processing in any other method than the Begin/EndRequest methods.</remarks>
        public void BeginRequest(IHttpContext context)
        {
        }

        /// <summary>
        /// End request is typically used for post processing. The response should already contain everything required.
        /// </summary>
        /// <param name="context">HTTP context</param>
        /// <remarks>
        /// <para>The last method that is executed in the pipeline.</para>
        /// Try to avoid throwing exceptions if you can. Let all modules have a chance to handle this method. You may break the processing in any other method than the Begin/EndRequest methods.</remarks>
        public void EndRequest(IHttpContext context)
        {
        }

        /// <summary>
        /// Authorize the request.
        /// </summary>
        /// <param name="context">HTTP context</param>
        /// <returns><see cref="ModuleResult.Stop"/> will stop all processing including <see cref="IHttpModule.EndRequest"/>.</returns>
        public ModuleResult Authenticate(IHttpContext context)
        {
            var user = _authenticator.Authenticate(context);
            if (user == null)
            {
                _authenticator.CreateChallenge(context);
                return ModuleResult.Stop;
            }

            context.User = user;
            return ModuleResult.Continue;
        }

        #endregion
    }
}