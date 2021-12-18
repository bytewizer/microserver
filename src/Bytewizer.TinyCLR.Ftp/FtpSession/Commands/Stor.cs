using System.IO;
using System.Net.Sockets;

using Bytewizer.TinyCLR.Ftp.Features;

namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        /// <summary>
        /// Implements the <c>STOR</c> command.
        /// </summary>
        private void Stor()
        {
            if (_context.Request.DataMode == DataMode.None)
            {
                BadSequenceOfCommands();
                return;
            }

            var file = _context.Request.Command.Argument;

            try
            {
                var feature = (SessionFeature)_context.Features.Get(typeof(SessionFeature));
                var marker = feature.RestPosition;

                // write to channel 
                _context.Channel.Write(150, "Opening connection for data transfer.");

                using (Stream ns = GetNetworkStream())
                {
                    var mode = (marker == 0) ? FileMode.Create : FileMode.Open;

                    using (FileStream fileStream = _fileProvider.OpenFileForWrite(file, mode))
                    {
                        ns.CopyTo(fileStream);
                        fileStream.Flush();
                    }
                }

                _context.Response.Write(226, "File upload succeeded.");
            }
            catch
            {
                _context.Response.Write(500, "STOR command failed.");
            }
        }
    }
}