using System.IO;
using System.Net.Sockets;

namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {

        private void Stor()
        {
            var argument = _context.Request.Command.Argument;

            try
            {
                // write to channel 
                _context.Channel.Write(150, "Status okay, opening data connection.");

                using (NetworkStream ns = GetNetworkStream())
                {
                    using (FileStream fileStream = _fileProvider.OpenFileForWrite(argument))
                    {
                        ns.CopyTo(fileStream);

                        fileStream.Flush();
                    }
                }

                _context.Response.Write(223, "Transfer complete.");

            }
            catch
            {
                _context.Response.Write(500, "Transfer incomplete.");
            }
        }
    }
}