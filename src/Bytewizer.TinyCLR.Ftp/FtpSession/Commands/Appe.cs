using System.IO;
using System.Net.Sockets;

namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        /// <summary>
        /// Implements the <c>APPE</c> command.
        /// </summary>
        private void Appe()
        {
            if (_context.Request.DataMode == DataMode.None)
            {
                BadSequenceOfCommands();
                return;
            }

            var file = _context.Request.Command.Argument;

            try
            {
                // write to channel 
                _context.Channel.Write(150, "Opening connection for data transfer.");

                using (Stream ns = GetNetworkStream())
                {
                    using (FileStream fileStream = _fileProvider.OpenFileForWrite(file, FileMode.Append))
                    {
                        fileStream.CopyTo(ns);
                    }
                }

                _context.Response.Write(226, "Uploaded file successfully.");
            }
            catch
            {
                _context.Response.Write(500, "APPE command failed.");
            }
        }
    }
}