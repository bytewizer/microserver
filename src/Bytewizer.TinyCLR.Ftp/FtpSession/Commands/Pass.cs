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
                var feature = (FtpAuthenticationFeature)_context.Features.Get(typeof(IFtpAuthenticationFeature));

                var allowAnonymous = (feature == null) ? true : feature.AllowAnonymous;

                if (_context.Request.Name.ToUpper() == "ANONYMOUS" && allowAnonymous)
                {
                    _context.Request.Authenticated = true;
                    _context.Response.Write(230, "User logged in.");
                    return;
                }

                if (feature != null)
                {
                    var identityProvider = feature.IdentityProvider;

                    var user = identityProvider.FindByName(_context.Request.Name);
                    if (user != null)
                    {
                        var password = Encoding.UTF8.GetBytes(_context.Request.Command.Argument);
                        var results = identityProvider.VerifyPassword(user, password);
                        if (results.Succeeded)
                        {
                            _context.Request.Authenticated = true;
                            _context.Response.Write(230, "User logged in.");
                        }
                        else
                        {
                            _context.Response.Write(530, "User cannot log in.", results.Errors, "End");
                        }

                        return;
                    }
                }

                ArrayList errors = new ArrayList();
                errors.Add("Logon failure: Unknown user name or bad password.");
                _context.Response.Write(530, "User cannot log in.", errors, "End");
            }
            catch
            {
                _context.Response.Write(500, "PASS command failed.");
            }
        }
    }
}