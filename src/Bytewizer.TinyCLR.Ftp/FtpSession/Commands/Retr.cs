using Bytewizer.TinyCLR.Ftp.Features;
using System.IO;
using System.Net.Sockets;

namespace Bytewizer.TinyCLR.Ftp
{
    internal partial class FtpSession
    {
        private void Retr()
        {
            if (_context.Request.DataMode == DataMode.None)
            {
                BadSequenceOfCommands();
                return;
            }

            var path = _context.Request.Command.Argument;
            
            var feature = (SessionFeature)_context.Features.Get(typeof(ISessionFeature));
            var marker = feature.RestMarker; 

            try
            {
                // write to channel 
                _context.Channel.Write(150, "Status okay, opening data connection.");

                using (NetworkStream ns = GetNetworkStream())
                {
                    using (FileStream fileStream = _fileProvider.OpenFileForRead(path))
                    {
                        if(marker != 0)
                        {
                            fileStream.Seek(marker, SeekOrigin.Begin);
                            marker = 0;
                        }
                        
                        fileStream.CopyTo(ns);
                    }
                }

                _context.Response.Write(223, "Transfer complete.");
            }
            catch
            {
                _context.Response.Write(500, "RETR command failed.");
            }
        }
    }
}