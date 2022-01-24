using Bytewizer.TinyCLR.Ftp.Features;

namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        /// <summary>
        /// Implements the <c>RNTO</c> command.
        /// </summary>
        private void Rnto()
        {
            try
            {
                var feature = (SessionFeature)_context.Features.Get(typeof(SessionFeature));
                var fromPath = feature.FromPath;
               
                if (string.IsNullOrEmpty(fromPath))
                {
                    _context.Response.Write(503, "RNTO must be preceded by a RNFR.");
                    return;
                }

                var toPath = _context.Request.Command.Argument;

                _fileProvider.Rename(fromPath, toPath);
                fromPath = null;

                _context.Response.Write(250, "Rename succeeded.");

            }
            catch
            {
                _context.Response.Write(500, "RNTO command failed.");
            }
        }
    }
}