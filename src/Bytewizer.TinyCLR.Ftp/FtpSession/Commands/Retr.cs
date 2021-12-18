using System.IO;
using System.Net.Sockets;

using Bytewizer.TinyCLR.Ftp.Features;

namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        /// <summary>
        /// Implements the <c>RETR</c> command.
        /// </summary>
        private void Retr()
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

                // write to channel 
                _context.Channel.Write(150, "Status okay, opening data connection.");

                using (Stream ns = GetNetworkStream())
                {
                    using (FileStream fileStream = _fileProvider.OpenFileForRead(file))
                    {
                        if (feature.RestPosition != 0)
                        {
                            fileStream.Seek(feature.RestPosition, SeekOrigin.Begin);
                            feature.RestPosition = 0;
                        }
                        
                        fileStream.CopyTo(ns);
                    }
                }

                _context.Response.Write(226, "File download succeeded.");
            }
            catch
            {
                _context.Response.Write(500, "RETR command failed.");
            }
        }
    }
}