using System.IO;
using System.Net.Sockets;

namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        private void Appe()
        {
            if (_context.Request.DataMode == DataMode.None)
            {
                BadSequenceOfCommands();
                return;
            }

            var path = _context.Request.Command.Argument;

            try
            {
                // write to channel 
                _context.Channel.Write(150, "Status okay, opening data connection.");

                using (NetworkStream ns = GetNetworkStream())
                {
                    using (FileStream fileStream = _fileProvider.OpenFileForWrite(path, FileMode.Append))
                    {
                        fileStream.CopyTo(ns);
                    }
                }

                _context.Response.Write(223, "Transfer complete.");
            }
            catch
            {
                _context.Response.Write(500, "APPE command failed.");
            }
        }
    }
}