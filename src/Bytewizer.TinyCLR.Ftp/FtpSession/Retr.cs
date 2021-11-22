using System.IO;
using System.Net.Sockets;

namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        private void Retr()
        {
            var argument = _context.Request.Command.Argument;

            // write to channel 
            _context.Channel.Write(150, "Status okay, opening data connection.");

            using (NetworkStream ns = GetNetworkStream())
            {
                using (FileStream fileStream = _fileProvider.OpenFileForRead(argument))
                {
                    fileStream.CopyTo(ns);
                }
            }

            _context.Response.Write(223, "Transfer complete.");
        }
    }
}