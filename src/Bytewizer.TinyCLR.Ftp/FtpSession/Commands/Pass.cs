using System;
using System.Text;
using System.Collections;

using Bytewizer.TinyCLR.Ftp.Features;

namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        /// <summary>
        /// Implements the <c>PASS</c> command.
        /// </summary>
        private void Pass()
        {
            if (string.IsNullOrEmpty(_context.Request.Name))
            {
                _context.Response.Write(503, $"Wrong sequence login aborted.");
                return;
            }

            try
            {
                var feature = (SessionFeature)_context.Features.Get(typeof(SessionFeature));

                if (_context.Request.Name.ToUpper() == "ANONYMOUS" && feature.AllowAnonymous)
                {
                    _context.Request.Authenticated = true;
                    SuccessfulLoginMessage();
                    return;
                }
                else
                {
                    var identityProvider = feature.IdentityProvider;

                    if (identityProvider != null)
                    {
                        var user = identityProvider.FindByName(_context.Request.Name);
                        if (user != null)
                        {
                            var password = Encoding.UTF8.GetBytes(_context.Request.Command.Argument);

                            var results = identityProvider.VerifyPassword(user, password);
                            if (results.Succeeded)
                            {
                                _context.Request.Authenticated = true;
                                SuccessfulLoginMessage();
                            }
                            else
                            {
                                FailedLoginMessage(results.Errors);
                            }
                        }
                    }

                    var errors = new ArrayList(1)
                    {
                        new Exception($"User '{_context.Request.Name}' cound not be located.")
                    };

                    FailedLoginMessage(errors);
                }
            }
            catch
            {
                _context.Response.Write(500, "PASS command failed.");
            }
        }

        private void SuccessfulLoginMessage()
        {
            _logger.LoginSucceeded(_context);
            _context.Response.Write(230, "User logged in.");
        }

        private void FailedLoginMessage(ArrayList errors)
        {
            foreach (Exception error in errors)
            {
                _logger.LoginFailed(_context, error.Message);
            }

            _context.Response.Write(530, "Logon failure: Unknown user name or bad password.");
        }
    }
}