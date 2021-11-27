using Bytewizer.TinyCLR.Ftp.Features;

namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        private void Rest()
        {
            try
            {
                var feature = (SessionFeature)_context.Features.Get(typeof(ISessionFeature));
                feature.RestMarker = int.Parse(_context.Request.Command.Argument);
                
                _context.Response.Write(350, $"350 Restarting at {feature.RestMarker}. Send STORE or RETRIEVE to initiate transfer.");
            }
            catch
            {
                _context.Response.Write(500, "REST command failed.");
            }
        }
    }
}