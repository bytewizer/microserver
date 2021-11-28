using Bytewizer.TinyCLR.Ftp.Features;

namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        /// <summary>
        /// Implements the <c>REST</c> command.
        /// </summary>
        private void Rest()
        {
            try
            {
                var feature = (SessionFeature)_context.Features.Get(typeof(ISessionFeature));
                feature.RestPosition = int.Parse(_context.Request.Command.Argument);
                
                _context.Response.Write(350, $"Restarting next transfer from position {feature.RestPosition}. Send STOR or RETR to initiate transfer.");
            }
            catch
            {
                _context.Response.Write(500, "REST command failed.");
            }
        }
    }
}