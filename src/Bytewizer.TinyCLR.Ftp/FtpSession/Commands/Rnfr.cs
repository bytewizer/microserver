using Bytewizer.TinyCLR.Ftp.Features;

namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        /// <summary>
        /// Implements the <c>RNFR</c> command.
        /// </summary>
        private void Rnfr()
        {
            try
            {
                var feature = (SessionFeature)_context.Features.Get(typeof(ISessionFeature));

                // TODO: Check to make sure directory or file exist
                // 550, "Directory doesn't exist."
                // 550, "Source entry doesn't exist."


                feature.FromPath = _context.Request.Command.Argument;

                _context.Response.Write(350, "Rename started waiting for RNTO.");

            }
            catch
            {
                _context.Response.Write(500, "RNFR command failed.");
            }
        }
    }
}