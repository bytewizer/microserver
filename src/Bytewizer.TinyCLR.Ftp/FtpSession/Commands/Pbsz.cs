using Bytewizer.TinyCLR.Ftp.Features;

namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        /// <summary>
        /// The <c>PBSZ</c> command handler.
        /// </summary>
        private void Pbsz()
        {
            //if (_context.Request.SecurityType == SecurityType.None)
            //{
            //    BadSequenceOfCommands();
            //    return;
            //}

            var size = _context.Request.Command.Argument;

            try
            {
                var feature = new SessionFeature()
                {
                    TlsBlockSize = int.Parse(size)
                };

                _context.Features.Set(typeof(SessionFeature), feature);
                _context.Response.Write(200, $"Protection buffer size set to {size}.");
            }
            catch
            {
                _context.Response.Write(500, "PBSZ command failed.");
            }
        }
    }
}